namespace QuizREST.Data.Entities;

public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    // Foreign key to reference the associated question
    public int QuestionId { get; set; }

}
