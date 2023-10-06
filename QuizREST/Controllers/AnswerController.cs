using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using QuizREST.Data.Dbs.Answers;
using QuizREST.Data.Dbs.Questions;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using QuizREST.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly LinkGenerator _linkGenerator;

        public AnswerController(IAnswerRepository answerRepository, IQuestionRepository questionRepository, LinkGenerator linkGenerator)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{quizId}/questions/{questionId}/answers")]
        public async Task<IActionResult> GetMany(int questionId)
        {
            var answers = await _answerRepository.GetAnswersForQuestionAsync(questionId);
            return Ok(answers);
        }

        [HttpGet("answer/{answerId}", Name = "GetAnswer")]
        public async Task<IActionResult> Get(int answerId)
        {
            HttpContext httpContext = HttpContext.Request.HttpContext;
            var answer = await _answerRepository.GetAnswerForQuestionAsync(answerId);

            if (answer == null)
            {
                return NotFound();
            }

            var links = CreateLinks(answer.Id, httpContext, _linkGenerator);
            var answerDto = new AnswerDto(answer.Id, answer.Text, answer.IsCorrect);

            return Ok(new { Resource = answerDto, Links = links });
        }

        [HttpPost("answer", Name = "CreateAnswer")]
        public async Task<ActionResult<AnswerDto>> Create(CreateAnswerDto createAnswerDto)
        {
            var answer = new Answer
            {
                Text = createAnswerDto.Text,
                IsCorrect = createAnswerDto.IsCorrect,
                QuestionId = createAnswerDto.questionId // Associate the answer with the specified question
            };

            await _answerRepository.CreateAsync(answer);

            // Create links for the created answer
            var httpContext = HttpContext.Request.HttpContext;
            var links = CreateLinks(answer.Id, httpContext, _linkGenerator);
            var answerDto = new AnswerDto(answer.Id, answer.Text, answer.IsCorrect);

            var resource = new ResourceDto<AnswerDto>(answerDto, links.ToArray());

            return CreatedAtAction(nameof(Get), new { answerId = answer.Id }, resource);
        }

        [HttpPut("answer/{answerId}", Name = "UpdateAnswer")]
        public async Task<IActionResult> Update(int answerId, UpdateAnswerDto updateAnswerDto)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(answerId);

            if (answer == null)
            {
                return NotFound();
            }

            var question = await _questionRepository.GetQuestionForQuizAsync(updateAnswerDto.questionId);

            if (question == null)
            {
                return BadRequest("The specified questionId does not exist.");
            }

            answer.Text = updateAnswerDto.Text;
            answer.IsCorrect = updateAnswerDto.IsCorrect;
            answer.QuestionId = updateAnswerDto.questionId;

            await _answerRepository.UpdateAsync(answer);

            var httpContext = HttpContext.Request.HttpContext;
            var links = CreateLinks(answer.Id, httpContext, _linkGenerator);
            var updatedAnswerDto = new AnswerDto(answer.Id, answer.Text, answer.IsCorrect);

            return Ok(new { Resource = updatedAnswerDto, Links = links });
        }

        [HttpDelete("answer/{answerId}", Name = "RemoveAnswer")]
        public async Task<IActionResult> Remove(int answerId)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(answerId);

            if (answer == null)
            {
                return NotFound();
            }

            await _answerRepository.RemoveAsync(answer);

            return NoContent();
        }

        static IEnumerable<LinksDto> CreateLinks(int answerId, HttpContext httpContext, LinkGenerator linkGenerator)
        {
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "GetAnswer", new { answerId }), "self", "GET");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "UpdateAnswer", new { answerId }), "edit", "PUT");
            yield return new LinksDto(linkGenerator.GetUriByName(httpContext, "DeleteAnswer", new { answerId }), "delete", "DELETE");
        }
    }
}
