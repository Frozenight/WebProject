using Microsoft.AspNetCore.Mvc;
using QuizREST.Data;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
    /*
    [HttpGet]
    public async Task<IEnumerable<QuizesDto>> GetMany()
    {
        var quizes = await _quizesRepository.GetManyAsync();
        return quizes.Select(o => new QuizesDto(o.Id, o.Name));
    }*/

    [HttpGet(Name = "GetQuizes")]
    public async Task<IEnumerable<QuizesDto>> GetManyPaging([FromQuery] QuizSearchParameters searchParameters)
    {
        var quizes = await _quizesRepository.GetManyAsync(searchParameters);

        var previousPageLink = quizes.HasPrevious ?
            CreateTopicsResourceUri(searchParameters,
                ResourceUriType.PreviousPage) : null;

        var nextPageLink = quizes.HasNext ?
            CreateTopicsResourceUri(searchParameters,
                ResourceUriType.NextPage) : null;

        var paginationMetadata = new
        {
            totalCount = quizes.TotalCount,
            pageSize = quizes.PageSize,
            currentPage = quizes.CurrentPage,
            totalPaages = quizes.TotalPages,
            previousPageLink,
            nextPageLink
        };

        // Pagination: 
        // {"resource": {}, "paging":{}}

        Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));

        return quizes.Select(o => new QuizesDto(o.Id, o.Name));
    }

    [HttpGet(("{quizId}"), Name = "GetQuiz")]
    public async Task<IActionResult> Get(int quizId)
    {
        var quiz = await _quizesRepository.GetAsync(quizId);



        // 404
        if (quiz == null)
        {
            return NotFound();
        }

        var links = CreateLinksForTopic(quizId);

        var topicDto = new QuizesDto(quiz.Id, quiz.Name);

        return Ok(new {Resource = topicDto, Links = links});
    }

    [HttpPost]
    public async Task<ActionResult<QuizesDto>> Create(CreateQuizDto createQuizDto)
    {
        var quiz = new Quiz { Name = createQuizDto.name };
        await _quizesRepository.CreateAsync(quiz);

        // 201
        return Created("", new QuizesDto(quiz.Id, quiz.Name));
        return CreatedAtAction("GetQuiz", new { quizId = quiz.Id }, new QuizesDto(quiz.Id, quiz.Name));
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

    [HttpDelete("{quizId}", Name = "DeleteTopic")]
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

    private IEnumerable<LinkDto> CreateLinksForTopic(int quizId)
    {
        yield return new LinkDto { Href = Url.Link("GetQuiz", new { quizId }), Rel = "self", Method = "GET" };
        yield return new LinkDto { Href = Url.Link("DeleteQuiz", new { quizId }), Rel = "delete_quiz", Method = "DELETE" };
    }

    private string? CreateTopicsResourceUri(
    QuizSearchParameters topicSearchParametersDto,
    ResourceUriType type)
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