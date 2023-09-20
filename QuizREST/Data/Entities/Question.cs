using System.Collections.Generic;

namespace QuizREST.Data.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }

    // Foreign key to reference the associated quiz
    public int QuizId { get; set; }

    // Navigation property to represent the collection of answers associated with the question
    public ICollection<Answer> Answers { get; set; }
}