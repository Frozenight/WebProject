using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizREST.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QuizREST.Auth.Model;

namespace QuizREST.Data
{
    public class ForumDBContext : IdentityDbContext<QuizRestUser>
    {
        private readonly IConfiguration _configuration;

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizes { get; set; }

        public ForumDBContext(DbContextOptions<ForumDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
