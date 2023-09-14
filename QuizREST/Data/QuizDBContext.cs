using Microsoft.EntityFrameworkCore;
using QuizREST.Data.Entities;

namespace QuizREST.Data;

public class QuizDBContext : DbContext
{
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Quiz> Quizes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source={localdb}\\MSSQLLocalDB; Initial Catalog=ForumDb2");
    }
}