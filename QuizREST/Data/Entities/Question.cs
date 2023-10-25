using QuizREST.Auth.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizREST.Data.Entities;

public class Question : IUserOwnedRecourse
{
    public int Id { get; set; }
    public string Text { get; set; }

    // Foreign key to reference the associated quiz
    public int QuizId { get; set; }
    [Required]
    public string UserId { get; set; }
    public QuizRestUser User { get; set; }
}