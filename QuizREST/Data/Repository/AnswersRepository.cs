using Microsoft.EntityFrameworkCore;
using QuizREST.Data.Dbs.Answers;
using QuizREST.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizREST.Data.Repository;

public interface IAnswerRepository
{
    Task CreateAsync(Answer answer);
    Task<Answer> GetAnswerForQuestionAsync(int answerId);
    Task<IReadOnlyList<Answer>> GetAnswersForQuestionAsync(int questionId);
    Task UpdateAsync(Answer answer);
    Task RemoveAsync(Answer answer);
    Task<Answer?> GetAnswerByIdAsync(int answerId);
}

public class AnswerRepository : IAnswerRepository
{
    private readonly ForumDBContext _forumDBContext;

    public AnswerRepository(ForumDBContext forumDBContext)
    {
        _forumDBContext = forumDBContext;
    }

    public async Task<Answer?> GetAnswerForQuestionAsync(int answerId)
    {
        return await _forumDBContext.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
    }

    public async Task<IReadOnlyList<Answer>> GetAnswersForQuestionAsync(int questionId)
    {
        return await _forumDBContext.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
    }

    public async Task CreateAsync(Answer answer)
    {
        _forumDBContext.Answers.Add(answer);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Answer answer)
    {
        _forumDBContext.Answers.Update(answer);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Answer answer)
    {
        _forumDBContext.Answers.Remove(answer);
        await _forumDBContext.SaveChangesAsync();
    }
    public async Task<Answer?> GetAnswerByIdAsync(int answerId)
    {
        return await _forumDBContext.Answers.FirstOrDefaultAsync(answer => answer.Id == answerId);
    }
}
