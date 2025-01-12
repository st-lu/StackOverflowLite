using Stackoverflow_Lite.Services.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Utils;
using System.Web;

namespace Stackoverflow_Lite.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(PostType postType, string emailAddress, string postContent, bool accepted, bool sendEmail = true)
        {
            if (sendEmail)
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("noreply.stackoverflowlite@gmail.com", ApplicationConstants.STACKOVERFLOWLITE);
                var subject = string.Format(ApplicationConstants.EMAIL_SUBJECT, accepted ? "accepted" : "rejected");
                var to = new EmailAddress(emailAddress);
                var plainTextContent = string.Format(accepted ? ApplicationConstants.ACCEPTED_EMAIL_CONTENT_PLAIN : ApplicationConstants.REJECTED_EMAIL_CONTENT_PLAIN, postType.ToString().ToLower(), postType.ToString().ToLower(), postContent);
                var htmlContent = string.Format(
                    accepted ? ApplicationConstants.ACCEPTED_EMAIL_CONTENT_HTML : ApplicationConstants.REJECTED_EMAIL_CONTENT_HTML, postType.ToString().ToLower(), postType.ToString().ToLower(), HttpUtility.HtmlEncode(postContent));
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                return;
            }
            Console.WriteLine(string.Format("Mock sending email to {0} (accepted = {1}, post = {2})",
                emailAddress,accepted,postContent));
        }
    }
}
