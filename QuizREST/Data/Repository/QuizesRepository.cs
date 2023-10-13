using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuizREST.Data.Dbs.Quizes;
using QuizREST.Data.Entities;
using QuizREST.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace QuizREST.Data.Repository;

public interface IQuizesRepository
{
    Task CreateAsync(Quiz question);
    Task<Quiz> GetAsync(int questionId);
    Task<IReadOnlyList<Quiz>> GetManyAsync();
    Task<PagedList<Quiz>> GetManyAsync(QuizSearchParameters quizSearchParameters);
    Task RemoveAsync(Quiz question);
    Task UpdateAsync(Quiz question);
    Task<Quiz?> GetQuizByIdAsync(int quizId);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task BeginTransactionAsync();
}

public class QuizesRepository : IQuizesRepository
{
    private readonly ForumDBContext _forumDBContext;
    private IDbContextTransaction _transaction;
    public QuizesRepository(ForumDBContext forumDBContext)
    {
        _forumDBContext = forumDBContext;
    }

    public async Task<Quiz?> GetAsync(int quizId)
    {
        return await _forumDBContext.Quizes.FirstOrDefaultAsync(o => o.Id == quizId);
    }

    public async Task<IReadOnlyList<Quiz>> GetManyAsync()
    {
        return await _forumDBContext.Quizes.ToListAsync();
    }

    public async Task<PagedList<Quiz>> GetManyAsync(QuizSearchParameters quizSearchParameters)
    {
        var queryable = _forumDBContext.Quizes.AsQueryable().OrderBy(o => o.Id);

        return await PagedList<Quiz>.CreateAsync(queryable, quizSearchParameters.PageNumber, quizSearchParameters.PageSize);
    }

    public async Task CreateAsync(Quiz quiz)
    {
        _forumDBContext.Quizes.Add(quiz);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        _forumDBContext.Quizes.Update(quiz);
        await _forumDBContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Quiz quiz)
    {
        _forumDBContext.Quizes.Remove(quiz);
        await _forumDBContext.SaveChangesAsync();
    }
    public async Task<Quiz?> GetQuizByIdAsync(int quizId)
    {
        return await _forumDBContext.Quizes.FirstOrDefaultAsync(quiz => quiz.Id == quizId);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _forumDBContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Handle exceptions or log them
            _transaction.Rollback();
            throw; // Re-throw the exception
        }
    }

    public async Task RollbackTransactionAsync()
    {
        _transaction.Rollback();
    }
}