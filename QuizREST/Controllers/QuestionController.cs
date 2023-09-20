using Microsoft.AspNetCore.Mvc;
using QuizREST.Data.Dbs.Questions;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System.Threading.Tasks;

namespace QuizREST.Controllers
{
    [ApiController]
    [Route("api/quizes/{quizId}/questions")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int quizId)
        {
            var questions = await _questionRepository.GetQuestionsForQuizAsync(quizId);
            return Ok(questions);
        }

        [HttpGet("{questionId}", Name = "GetQuestion")]
        public async Task<IActionResult> Get(int quizId, int questionId)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(quizId, questionId);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create(int quizId, CreateQuestionDto createQuestionDto)
        {
            var question = new Question
            {
                Text = createQuestionDto.Text,
                QuizId = quizId
            };

            await _questionRepository.CreateAsync(question);

            return CreatedAtAction("GetQuestion", new { quizId, questionId = question.Id }, question);
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> Update(int quizId, int questionId, UpdateQuestionDto updateQuestionDto)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(quizId, questionId);

            if (question == null)
            {
                return NotFound();
            }

            question.Text = updateQuestionDto.Text;

            await _questionRepository.UpdateAsync(question);

            return Ok(question);
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> Remove(int quizId, int questionId)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(quizId, questionId);

            if (question == null)
            {
                return NotFound();
            }

            await _questionRepository.RemoveAsync(question);

            return NoContent();
        }
    }
}
