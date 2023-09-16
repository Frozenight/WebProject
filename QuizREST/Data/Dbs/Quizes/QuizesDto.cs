namespace QuizREST.Data.Dbs.Quizes;

public record QuizesDto(int id, string name);
public record CreateQuizDto(string name);
public record UpdateQuizDto(string name);