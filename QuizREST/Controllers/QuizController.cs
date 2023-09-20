using Microsoft.AspNetCore.Mvc;
using QuizREST.Data;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizesRepository _quizesRepository;

        public QuizController(IQuizesRepository quizesRepository)
        {
            _quizesRepository = quizesRepository;
        }

        [HttpGet(Name = "GetQuizes")]
        public async Task<IActionResult> GetManyPaging([FromQuery] QuizSearchParameters searchParameters)
        {
            var quizes = await _quizesRepository.GetManyAsync(searchParameters);

            var previousPageLink = quizes.HasPrevious ?
                CreateTopicsResourceUri(searchParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = quizes.HasNext ?
                CreateTopicsResourceUri(searchParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = quizes.TotalCount,
                pageSize = quizes.PageSize,
                currentPage = quizes.CurrentPage,
                totalPages = quizes.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));

            var quizesDtoList = quizes.Select(quiz => new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate));

            return Ok(quizesDtoList);
        }

        [HttpGet("{quizId}", Name = "GetQuiz")]
        public async Task<IActionResult> Get(int quizId)
        {
            var quiz = await _quizesRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var links = CreateLinksForQuiz(quizId);

            var quizDto = new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate);

            return Ok(new { Resource = quizDto, Links = links });
        }

        [HttpPost]
        public async Task<ActionResult<QuizesDto>> Create(CreateQuizDto createQuizDto)
        {
            var quiz = new Quiz
            {
                Name = createQuizDto.Name,
                Description = createQuizDto.Description,
                Category = createQuizDto.Category,
                CreatedDate = DateTime.Now
            };

            await _quizesRepository.CreateAsync(quiz);

            return CreatedAtAction("GetQuiz", new { quizId = quiz.Id }, new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate));
        }

        [HttpPut("{quizId}")]
        public async Task<ActionResult<QuizesDto>> Update(int quizId, UpdateQuizDto updateQuizDto)
        {
            var quiz = await _quizesRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            quiz.Name = updateQuizDto.Name;
            quiz.Description = updateQuizDto.Description;
            quiz.Category = updateQuizDto.Category;

            await _quizesRepository.UpdateAsync(quiz);

            return Ok(new QuizesDto(quiz.Id, quiz.Name, quiz.Description, quiz.Category, quiz.CreatedDate));
        }

        [HttpDelete("{quizId}", Name = "DeleteQuiz")]
        public async Task<ActionResult> Remove(int quizId)
        {
            var quiz = await _quizesRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            await _quizesRepository.RemoveAsync(quiz);

            return NoContent();
        }

        private IEnumerable<LinkDto> CreateLinksForQuiz(int quizId)
        {
            yield return new LinkDto { Href = Url.Link("GetQuiz", new { quizId }), Rel = "self", Method = "GET" };
            yield return new LinkDto { Href = Url.Link("DeleteQuiz", new { quizId }), Rel = "delete_quiz", Method = "DELETE" };
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
