using Microsoft.AspNetCore.Mvc;

namespace QuizREST.Controllers;



[ApiController]
[Route("api/quizes/{quizId}/questions/{questionId}/answers")]
public class AnswerController : ControllerBase
{
    public void GetMany()
    {

    }

    [HttpGet]
    [Route("{answerId}")]
    public void Get(int quizId, int questionId, int answerId)
    {
        // Topic exists + post exists
        // else not found()
    }
    /*
    [HttpGet]
    [Route("{answerId}")]
    public void Get([FromQuery] SearchAnswerParameters parameters)
    {
        // Topic exists + post exists
        // else not found()
    }*/

    public record SearchAnswerParameters(int quizId, int questionId, int answerId)
    {

    }


    public void Create() { }

    public void Update() { }

    public void Remove() { }
}