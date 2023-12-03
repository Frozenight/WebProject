using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using QuizREST.Auth.Model;
using QuizREST.Data;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using QuizREST.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes")]
    public class QuizController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizesRepository _quizesRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public QuizController(IQuizesRepository quizesRepository, IAnswerRepository answerRepository, LinkGenerator linkGenerator, 
            HttpContextAccessor httpContextAccessor, IQuestionRepository questionRepository, IAuthorizationService authorizationService)
        {
            _answerRepository = answerRepository;
            _quizesRepository = quizesRepository;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _questionRepository = questionRepository;
            _authorizationService = authorizationService;
        }

        [HttpGet(Name = "GetQuizes")]
        public async Task<IActionResult> GetManyPaging()
        {
            var quizes = await _quizesRepository.GetManyAsync();

            var quizesDtoList = quizes.Select(quiz => new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate));

            return Ok(quizesDtoList);
        }

        [HttpGet("{quizId}", Name = "GetQuiz")]
        public async Task<IActionResult> Get(int quizId)
        {
            HttpContext httpContext = HttpContext.Request.HttpContext;
            var quiz = await _quizesRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var links = CreateLinks(quiz.Id, httpContext, _linkGenerator);

            var quizDto = new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate);

            return Ok(new { Resource = quizDto, Links = links });
        }

        [HttpPost(Name = "CreateQuiz")]
        [Authorize(Roles = QuizRoles.Admin)]
        public async Task<ActionResult<QuizesDto>> Create([FromBody] CreateQuizDto createQuizDto)
        {
            //var httpContext = _httpContextAccessor.HttpContext;
            HttpContext httpContext = HttpContext.Request.HttpContext;
            var quiz = new Quiz
            {
                Name = createQuizDto.Name,
                Description = createQuizDto.Description,
                Category = createQuizDto.Category,
                CreatedDate = DateTime.Now,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            await _quizesRepository.CreateAsync(quiz);

            var links = CreateLinks(quiz.Id, httpContext, _linkGenerator);
            var quizDto = new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate);

            var resource = new ResourceDto<QuizesDto>(quizDto, links.ToArray());

            return CreatedAtAction(nameof(Get), new { quizId = quiz.Id }, resource);
        }

        [HttpPut("{quizId}", Name = "UpdateQuiz")]
        [Authorize(Roles = QuizRoles.Admin)]
        public async Task<ActionResult<QuizesDto>> Update(int quizId, UpdateQuizDto updateQuizDto)
        {
            var quiz = await _quizesRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, quiz, PolicyNames.RecouseOwner);
            if (!authorizationResult.Succeeded)
                return Forbid();


            quiz.Name = updateQuizDto.Name;
            quiz.Description = updateQuizDto.Description;
            quiz.Category = updateQuizDto.Category;

            await _quizesRepository.UpdateAsync(quiz);

            // Create links for the updated quiz
            var httpContext = HttpContext.Request.HttpContext;
            var links = CreateLinks(quiz.Id, httpContext, _linkGenerator);

            var updatedQuizDto = new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate);

            return Ok(new { Resource = updatedQuizDto, Links = links });
        }

        [HttpDelete("{quizId}", Name = "DeleteQuiz")]
        [Authorize(Roles = QuizRoles.Admin)]
        public async Task<ActionResult> Remove(int quizId)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var quiz = await _quizesRepository.GetAsync(quizId);

                    if (quiz == null)
                    {
                        return NotFound();
                    }

                    var questions = await _questionRepository.GetQuestionsForQuizAsync(quizId);

                    if (questions != null && questions.Any())
                    {
                        foreach (var question in questions)
                        {
                            var answers = await _answerRepository.GetAnswersForQuestionAsync(question.Id);

                            if (answers != null && answers.Any())
                            {
                                // Delete all associated answers
                                foreach (var answer in answers)
                                {
                                    await _answerRepository.RemoveAsync(answer);
                                }
                            }

                            await _questionRepository.RemoveAsync(question);
                        }
                    }

                    await _quizesRepository.RemoveAsync(quiz);

                    transactionScope.Complete(); // Commit the transaction.

                    return NoContent();
                }
                catch (Exception)
                {
                    // Handle the exception, log it, etc.
                    return StatusCode(500); // Or another appropriate error response
                }
            }
        }




        static IEnumerable<LinksDto> CreateLinks(int quizId, HttpContext httpContext, LinkGenerator linkGenerator)
        {
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "GetQuiz", new { quizId}), "self", "GET");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "UpdateQuiz", new { quizId }), "edit", "PUT");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "DeleteQuiz", new { quizId }), "delete", "DELETE");
        }

        private string? CreateTopicsResourceUri(QuizSearchParameters topicSearchParametersDto, ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetQuizes",
                    new
                    {
                        pageNumber = topicSearchParametersDto.PageNumber - 1,
                        pageSize = topicSearchParametersDto.PageSize,
                    }),
                ResourceUriType.NextPage => Url.Link("GetQuizes",
                    new
                    {
                        pageNumber = topicSearchParametersDto.PageNumber + 1,
                        pageSize = topicSearchParametersDto.PageSize,
                    }),
                _ => Url.Link("GetQuizes",
                    new
                    {
                        pageNumber = topicSearchParametersDto.PageNumber,
                        pageSize = topicSearchParametersDto.PageSize,
                    })
            };
        }
    }
}
