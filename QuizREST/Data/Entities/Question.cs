namespace QuizREST.Data.Entities;

public class Question
{
    public int Id { get; set; }

    public string Name { get; set; }
    public Quiz Quiz { get; set; }
}