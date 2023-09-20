namespace QuizREST.Data.Dbs.Answers
{
    public record AnswerDto(int Id, string Text, bool IsCorrect);
    public record CreateAnswerDto(string Text, bool IsCorrect);
    public record UpdateAnswerDto(string Text, bool IsCorrect);
}
