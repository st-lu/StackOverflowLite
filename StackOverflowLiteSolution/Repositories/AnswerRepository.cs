using Microsoft.EntityFrameworkCore;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Utils;

namespace Stackoverflow_Lite.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public AnswerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Answer> CreateAnswerAsync(Answer answer)
    {
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return answer;
    }

    public async Task<Answer> EditAnswerAsync(Guid answerId, AnswerRequest answerRequest)
    {
        var answer = await FindAnswerAsyncById(answerId);
        answer.Content = answerRequest.Content;
        await _context.SaveChangesAsync();
        return answer;
    }
        
    public async Task DeleteAnswerAsync(Guid answerId)
    {
        var answer = await FindAnswerAsyncById(answerId);
        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
    }

    public async Task<Guid> GetAuthorIdFromAnswerIdAsync(Guid answerId)
    {
        var userId = await _context.Answers
            .Where(a => a.Id == answerId)
            .Where(a => a.IsVisible)
            .Select(a => a.UserId)
            .FirstOrDefaultAsync();

        if (userId == Guid.Empty)
            throw new EntityNotFound(string.Format(ApplicationConstants.ANSWER_NOT_FOUND_MESSAGE, answerId.ToString()));

        return userId; 
    }

    private async Task<Answer> FindAnswerAsyncById(Guid answerId)
    {
        var answer = await _context.Answers.FindAsync(answerId);
        if (answer == null)
            throw new EntityNotFound(string.Format(ApplicationConstants.ANSWER_NOT_FOUND_MESSAGE,answerId.ToString()));
        return answer;
    }
    
    public async Task UpdateAnswerTextCategoryAsync(Guid answerId, TextCategory textCategory)
    {
        var answer = await FindAnswerAsyncById(answerId);
        answer.TextCategory = textCategory;
        answer.IsVisible = (textCategory == TextCategory.HateSpeech || textCategory == TextCategory.OffensiveLanguage) ? false: true;
        await _context.SaveChangesAsync();
    }

    public async Task<Answer> VoteAnswerAsync(Guid answerId, int value)
    {
        var answer = await FindAnswerAsyncById(answerId);
        answer.Score += value;
        await _context.SaveChangesAsync();

        return answer;
    }


    
}