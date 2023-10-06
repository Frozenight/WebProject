using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using QuizREST.Data;
using QuizREST.Data.Dbs.Questions;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using QuizREST.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizesRepository _quizesRepository;
        private readonly LinkGenerator _linkGenerator;

        public QuestionController(IQuestionRepository questionRepository, IQuizesRepository quizesRepository, LinkGenerator linkGenerator)
        {
            _questionRepository = questionRepository;
            _quizesRepository = quizesRepository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [Route("{quizId}/questions")]
        public async Task<IActionResult> GetMany(int quizId)
        {
            var questions = await _questionRepository.GetQuestionsForQuizAsync(quizId);
            return Ok(questions);
        }

        [HttpGet("question/{questionId}", Name = "GetQuestion")]
        public async Task<IActionResult> Get(int questionId)
        {
            HttpContext httpContext = HttpContext.Request.HttpContext;
            var question = await _questionRepository.GetQuestionForQuizAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            var links = CreateLinks(question.Id, httpContext, _linkGenerator);
            var quiestionDto = new QuestionDto(question.Id, question.Text);

            return Ok(new { Resource = quiestionDto, Links = links });
        }

        [HttpPost("question", Name = "CreateQuestion")]
        public async Task<ActionResult<QuestionDto>> Create(CreateQuestionDto createQuestionDto)
        {
            HttpContext httpContext = HttpContext.Request.HttpContext;
            var question = new Question
            {
                Text = createQuestionDto.Text,
                QuizId = createQuestionDto.quizId
            };

            await _questionRepository.CreateAsync(question);

            // Create links for the created question
            var links = CreateLinks(question.Id, httpContext, _linkGenerator);
            var questionDto = new QuestionDto(question.Id, question.Text);

            var resource = new ResourceDto<QuestionDto>(questionDto, links.ToArray());

            return CreatedAtAction(nameof(Get), new { questionId = question.Id }, resource);
        }

        [HttpPut("question/{questionId}", Name = "UpdateQuestion")]
        public async Task<IActionResult> Update(int questionId, UpdateQuestionDto updateQuestionDto)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            // Check if the provided quizId exists in the database
            var quiz = await _quizesRepository.GetQuizByIdAsync(updateQuestionDto.quizId);

            if (quiz == null)
            {
                return BadRequest("The specified quizId does not exist.");
            }

            question.Text = updateQuestionDto.Text;
            question.QuizId = updateQuestionDto.quizId;

            await _questionRepository.UpdateAsync(question);

            var httpContext = HttpContext.Request.HttpContext;
            var links = CreateLinks(question.Id, httpContext, _linkGenerator);

            var updatedQuestionDto = new QuestionDto(question.Id, question.Text);

            return Ok(new { Resource = updatedQuestionDto, Links = links });
        }

        [HttpDelete("question/{questionId}", Name = "DeleteQuestion")]
        public async Task<IActionResult> Remove(int questionId)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            await _questionRepository.RemoveAsync(question);

            return NoContent();
        }

        static IEnumerable<LinksDto> CreateLinks(int questionId, HttpContext httpContext, LinkGenerator linkGenerator)
        {
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "GetQuestion", new { questionId }), "self", "GET");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "UpdateQuestion", new { questionId }), "edit", "PUT");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "DeleteQuestion", new { questionId }), "delete", "DELETE");
        }
    }
}
