namespace QuizREST.Data.Dbs.Questions
{
    public record QuestionDto(int Id, string Text);
    public record CreateQuestionDto(string Text);
    public record UpdateQuestionDto(string Text);
}