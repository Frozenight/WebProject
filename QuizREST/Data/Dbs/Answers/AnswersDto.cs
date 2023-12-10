namespace QuizREST.Data.Dbs.Answers
{
    public record AnswerDto(int Id, string Text, bool IsCorrect, int questionId);
    public record CreateAnswerDto(string Text, bool IsCorrect, int questionId);
    public record UpdateAnswerDto(string Text, bool IsCorrect, int questionId);
}
