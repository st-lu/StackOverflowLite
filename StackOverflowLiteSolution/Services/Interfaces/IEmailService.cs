using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(PostType postType, string emailAddress, string postContent, bool accepted, bool sendEmail = true);
    }
}
