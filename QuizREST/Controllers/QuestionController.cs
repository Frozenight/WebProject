using Microsoft.AspNetCore.Mvc;
using QuizREST.Data;
using QuizREST.Data.Dbs.Questions;
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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizesRepository _quizesRepository;

        public QuestionController(IQuestionRepository questionRepository, IQuizesRepository quizesRepository)
        {
            _questionRepository = questionRepository;
            _quizesRepository = quizesRepository;
        }

        [HttpGet]
        [Route("{quizId}/questions")]
        public async Task<IActionResult> GetMany(int quizId)
        {
            var questions = await _questionRepository.GetQuestionsForQuizAsync(quizId);
            return Ok(questions);
        }

        [HttpPost("question")]
        public async Task<ActionResult<QuestionDto>> Create(CreateQuestionDto createQuestionDto)
        {
            var question = new Question
            {
                Text = createQuestionDto.Text,
                QuizId = createQuestionDto.quizId
            };

            await _questionRepository.CreateAsync(question);

            return CreatedAtAction(nameof(Get), new { questionId = question.Id }, question);
        }

        [HttpGet("question/{questionId}", Name = "GetQuestion")]
        public async Task<IActionResult> Get(int questionId)
        {
            var question = await _questionRepository.GetQuestionForQuizAsync(questionId);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpPut("question/{questionId}")]
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

            return Ok(question);
        }

        [HttpDelete("question/{questionId}")]
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
    }
}
