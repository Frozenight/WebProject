namespace QuizREST.Data.Dbs.Questions
{
    public record QuestionDto(int Id, string Text, int quizId);
    public record CreateQuestionDto(string Text, int quizId);
    public record UpdateQuestionDto(string Text, int quizId);
}