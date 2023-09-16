using Microsoft.AspNetCore.Mvc;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizREST.Controllers;

[ApiController]
[Route("api/quizes")]
public class QuizController : ControllerBase
{
    private readonly IQuizesRepository _quizesRepository;

    public QuizController(IQuizesRepository quizesRepository) {
        _quizesRepository = quizesRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<QuizesDto>> GetMany()
    {
        var quizes = await _quizesRepository.GetManyAsync();
        return quizes.Select(o => new QuizesDto(o.Id, o.Name));
    }

    [HttpGet]
    public async Task<IEnumerable<QuizesDto>> GetManyPaging([FromQuery] QuizSearchParameters searchParameters)
    {
        var quizes = await _quizesRepository.GetManyAsync(searchParameters);
        return quizes.Select(o => new QuizesDto(o.Id, o.Name));
    }

    [HttpGet]
    [Route("{quizId}", Name = "GetQuiz")]
    public async Task<ActionResult<QuizesDto>> Get(int quizId)
    {
        var quiz = await _quizesRepository.GetAsync(quizId);
        // 404
        if (quiz == null)
        {
            return NotFound();
        }
        return new QuizesDto(quiz.Id, quiz.Name);
    }

    [HttpPost]
    public async Task<ActionResult<QuizesDto>> Create(CreateQuizDto createQuizDto)
    {
        var quiz = new Quiz { Name = createQuizDto.name };
        await _quizesRepository.CreateAsync(quiz);

        // 201
        return Created("", new QuizesDto(quiz.Id, quiz.Name));
        return CreatedAtAction("GetQuiz", new { quizId = quiz.Id}, new QuizesDto(quiz.Id, quiz.Name));
    }

    [HttpPut]
    [Route("{quizId}")]
    public async Task<ActionResult<QuizesDto>> Update(int quizId, UpdateQuizDto updateQuizDto)
    {
        var quiz = await _quizesRepository.GetAsync(quizId);
        // 404
        if (quiz == null)
        {
            return NotFound();
        }

        quiz.Name = updateQuizDto.name;
        await _quizesRepository.UpdateAsync(quiz);

        return Ok(new QuizesDto(quiz.Id, quiz.Name));
    }

    [HttpDelete]
    [Route("{quizId}")]
    public async Task<ActionResult> Remove(int quizId)
    {
        var quiz = await _quizesRepository.GetAsync(quizId);
        // 404
        if (quiz == null)
        {
            return NotFound();
        }

        await _quizesRepository.RemoveAsync(quiz);

        // 204
        return NoContent();
    }
}