using System;

namespace QuizREST.Data.Dbs.Quizes;

public record QuizesDto(int Id, string Name, string Description, string Category, DateTime CreatedDate);
public record CreateQuizDto(string Name, string Description, string Category);
public record UpdateQuizDto(string Name, string Description, string Category);
