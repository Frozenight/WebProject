using QuizREST.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace QuizREST.Data.Entities;

public class Answer : IUserOwnedRecourse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    // Foreign key to reference the associated question
    public int QuestionId { get; set; }
    [Required]
    public string UserId { get; set; }
    public QuizRestUser User { get; set; }

}
