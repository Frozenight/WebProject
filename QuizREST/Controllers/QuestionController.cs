using Microsoft.AspNetCore.Mvc;

namespace QuizREST.Controllers;

[ApiController]
[Route("api/quizes/{quizId}/questions")]
public class QuestionController : ControllerBase
{

    public void GetMany()
    {
        
    }

    [HttpGet]
    [Route("{questionId}")]
    public void Get(int quizId, int questionId)
    {
        // Topic exists + post exists
        // else not found()
    }


    public void Create() { }

    public void Update() { }

    public void Remove() { }
}