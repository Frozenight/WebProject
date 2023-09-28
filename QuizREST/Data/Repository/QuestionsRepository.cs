using Microsoft.EntityFrameworkCore;
using QuizREST.Data.Dbs.Questions;
using QuizREST.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizREST.Data.Repository;

public interface IQuestionRepository
{
    Task CreateAsync(Question question);
    Task<Question> GetQuestionForQuizAsync(int questionId);
    Task<IReadOnlyList<Question>> GetQuestionsForQuizAsync(int quizId);
    Task UpdateAsync(Question question);
    Task RemoveAsync(Question question);
}

public class QuestionRepository : IQuestionRepository
{
    private readonly ForumDBContext _forumDBContext;

    public QuestionRepository(ForumDBContext forumDBContext)
    {
        _forumDBContext = forumDBContext;
    }

    public async Task<Question?> GetQuestionForQuizAsync(int questionId)
    {
        return await _forumDBContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<IReadOnlyList<Question>> GetQuestionsForQuizAsync(int quizId)
    {
        return await _forumDBContext.Questions.Where(q => q.QuizId == quizId).ToListAsync();
    }

    public async Task CreateAsync(Question question)
    {
        _forumDBContext.Questions.Add(question);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Question question)
    {
        _forumDBContext.Questions.Update(question);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Question question)
    {
        _forumDBContext.Questions.Remove(question);
        await _forumDBContext.SaveChangesAsync();
    }
}
