namespace QuizREST.Data.Entities;

public class Answer
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool isCorrect { get; set; }

    public Question Question { get; set; }
}