using System.Collections.Generic;
using System;

namespace QuizREST.Data.Entities;

public class Quiz
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public DateTime CreatedDate { get; set; }
}