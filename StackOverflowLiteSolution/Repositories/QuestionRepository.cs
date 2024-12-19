using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Utils;

namespace Stackoverflow_Lite.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Question> CreateQuestionAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task<Question?> GetQuestionAsync(Guid questionId)
    {
        var question = await _context.Questions
            .Include(q => q.User)
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == questionId);
        if (question == null)
            throw new EntityNotFound(string.Format(ApplicationConstants.QUESTION_NOT_FOUND_MESSAGE, questionId.ToString()));
        return question;
    }

    public async Task DeleteQuestionAsync(Guid questionId)
    {
        var question = await FindQuestionAsyncById(questionId);
        
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        
    }

    public async Task<Guid> GetAuthorIdFromQuestionIdAsync(Guid questionId)
    {
        var userId = await _context.Questions
            .Where(q => q.Id == questionId)
            .Select(q => q.UserId)
            .FirstOrDefaultAsync();

        if (userId == Guid.Empty)
            throw new EntityNotFound(string.Format(ApplicationConstants.QUESTION_NOT_FOUND_MESSAGE, questionId.ToString()));

        return userId;
    }
    
    public async Task<Question> EditQuestionAsync(Guid questionId, QuestionRequest questionRequest)
    {

        var question = await FindQuestionAsyncById(questionId);
        question.Content = questionRequest.Content;
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task<List<Question>> GetQuestionsBatchAsync(int offset, int size)
    {
        
        // function to return the list of questions (ordered desc by the number of UNIQUE viewers) 
        return await _context.Questions
            .OrderBy(q => -q.ViewsCount)
            .Skip(offset)
            .Take(size)
            .ToListAsync();
    }

    public async Task TryToIncrementViewQuestionCount(Question question, Guid userId)
    {
        var userQuestionView = await _context.UserQuestionViews.FirstOrDefaultAsync(uqv => uqv.UserId == userId && uqv.QuestionId == question.Id);
        // check if the current user already clicked on the question
        if (userQuestionView == null)
        {
            _context.UserQuestionViews.Add(new UserQuestionView
            {
                UserId = userId,
                QuestionId = question.Id
            });
            question.ViewsCount++;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<Question> FindQuestionAsyncById(Guid questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
    
        if (question == null)
            throw new EntityNotFound(string.Format(ApplicationConstants.QUESTION_NOT_FOUND_MESSAGE, questionId.ToString()));
        return question;
    }

    public async Task UpdateQuestionTextCategoryAsync(Guid questionId, TextCategory textCategory, bool visible)
    {
        var question = await FindQuestionAsyncById(questionId);
        question.TextCategory = textCategory;
        question.IsVisible = visible;
        await _context.SaveChangesAsync();
    }
}