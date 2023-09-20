using Microsoft.AspNetCore.Mvc;
using QuizREST.Data.Dbs.Answers;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes/{quizId}/questions/{questionId}/answers")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int quizId, int questionId)
        {
            var answers = await _answerRepository.GetAnswersForQuestionAsync(questionId);
            return Ok(answers);
        }

        [HttpGet("{answerId}", Name = "GetAnswer")]
        public async Task<IActionResult> Get(int quizId, int questionId, int answerId)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(questionId, answerId);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDto>> Create(int quizId, int questionId, CreateAnswerDto createAnswerDto)
        {
            var answer = new Answer
            {
                Text = createAnswerDto.Text,
                IsCorrect = createAnswerDto.IsCorrect,
                QuestionId = questionId // Associate the answer with the specified question
            };

            await _answerRepository.CreateAsync(questionId, answer);

            return CreatedAtAction("GetAnswer", new { quizId, questionId, answerId = answer.Id }, answer);
        }

        [HttpPut("{answerId}")]
        public async Task<IActionResult> Update(int quizId, int questionId, int answerId, UpdateAnswerDto updateAnswerDto)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(questionId, answerId);

            if (answer == null)
            {
                return NotFound();
            }

            answer.Text = updateAnswerDto.Text;
            answer.IsCorrect = updateAnswerDto.IsCorrect;

            await _answerRepository.UpdateAsync(answer);

            return Ok(answer);
        }

        [HttpDelete("{answerId}")]
        public async Task<IActionResult> Remove(int quizId, int questionId, int answerId)
        {
            var answer = await _answerRepository.GetAnswerForQuestionAsync(questionId, answerId);

            if (answer == null)
            {
                return NotFound();
            }

            await _answerRepository.RemoveAsync(answer);

            return NoContent();
        }
    }
}
