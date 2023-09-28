using Microsoft.AspNetCore.Mvc;
using QuizREST.Data.Dbs.Answers;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;

        public AnswerController(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
        }

        [HttpGet("{quizId}/questions/{questionId}/answers")]
        public async Task<IActionResult> GetMany(int questionId)
        {
            var answers = await _answerRepository.GetAnswersForQuestionAsync(questionId);
            return Ok(answers);
        }

        [HttpPost("answer")]
        public async Task<ActionResult<AnswerDto>> Create(CreateAnswerDto createAnswerDto)
        {
            var answer = new Answer
            {
                Text = createAnswerDto.Text,
                IsCorrect = createAnswerDto.IsCorrect,
                QuestionId = createAnswerDto.questionId // Associate the answer with the specified question
            };

            await _answerRepository.CreateAsync(answer);

            return CreatedAtAction(nameof(Get), new { answerId = answer.Id }, answer);
        }

        [HttpGet("answer/{answerId}", Name = "GetAnswer")]
        public async Task<IActionResult> Get(int answerId)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(answerId);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        [HttpPut("answer/{answerId}")]
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

            return Ok(answer);
        }

        [HttpDelete("answer/{answerId}")]
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
    }
}
