using System.CodeDom;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Stackoverflow_Lite.Utils;


public interface ApplicationConstants
{
    public const string OIDC_CLAIMS_EXTRACTION_ERROR = "Invalid token, couldn't extract the required claims. (sub claim: {0}, preffered_username claim: {1})";
    public const string OIDC_MAPPING_NOT_CREATED = "Couldn't find the user representation from the extracted sub claim. Consider creating the user making a POST request against the '/user' endpoint ";
    public const string OIDC_MAPPING_ALREADY_CREATED = "You cannot recreate an active user (mapping between OIDC user and application user was already done)";

    public const string OPERATION_NOT_ALLOWED_MESSAGE = "Current user is not allowed to modify a resource they are not an owner of";
    public const string QUESTION_NOT_FOUND_MESSAGE = "Question with id {0} cannot be found in the DB";
    public const string QUESTION_SUCCESSFULLY_DELETED = "Question was successfully deleted";
    public const string ANSWER_SUCCESSFULLY_DELETED = "Answer was successfully deleted";
    public const string ANSWER_NOT_FOUND_MESSAGE = "Answer not found!";
    public const string HATE_SPEECH_TEXT = "'{0}' was categorized as hate speech. Your {1} was not posted";
    public const string OFFENSIVE_SPEECH_TEXT = "'{0}' was categorized as offensive. Your {1} was not posted";
    public const string TEXT_ACCPETED = "'{0}' was accepted. Your {1} is visible.";
    public const string STACKOVERFLOWLITE = "StackOverflowLite";
    public const string EMAIL_SUBJECT = "Your post has been {0}";
    public const string ACCEPTED_EMAIL_CONTENT_PLAIN = "Your {0} has been accepted.\n Other users can now see it on the feed!\n ------ Your {1} was: {2} -----";
    public const string ACCEPTED_EMAIL_CONTENT_HTML = @"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        h1 {{
            color: #2c3e50;
            border-bottom: 2px solid #3498db;
            padding-bottom: 10px;
        }}
        .highlight {{
            background-color: #f1c40f;
            padding: 2px 5px;
            border-radius: 3px;
        }}
        .content {{
            background-color: #ecf0f1;
            border-left: 5px solid #3498db;
            padding: 10px;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <h1>Your {0} has been accepted</h1>
    <p>
        Your {0} has been <span class=""highlight"">accepted</span>.
    </p>
    <p>
         Other users can now see it on the feed!
    </p>
    <div class=""content"">
        <strong>Your {1} was: </strong>
        <p>{2}</p>
    </div>
</body>
</html>";
    public const string REJECTED_EMAIL_CONTENT_PLAIN = "Your {0} has been rejected.\n Your post violates our community guidelines. Think this is a mistake? Email us at: stackoverflowlite.customersupport@gmail.com.\n ------ Your {1} was: {2} -----";
    public const string REJECTED_EMAIL_CONTENT_HTML = @"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        h1 {{
            color: #2c3e50;
            border-bottom: 2px solid #3498db;
            padding-bottom: 10px;
        }}
        .highlight {{
            background-color: #f1c40f;
            padding: 2px 5px;
            border-radius: 3px;
        }}
        .content {{
            background-color: #ecf0f1;
            border-left: 5px solid #3498db;
            padding: 10px;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <h1>Your {0} has been rejected</h1>
    <p>
        Your {0} has been <span class=""highlight"">rejected</span>.
    </p>
    <p>
         Your post violates our community guidelines. Think this is a mistake? Email us at: stackoverflowlite.customersupport@gmail.com
    </p>
    <div class=""content"">
        <strong>Your {1} was: </strong>
        <p>{2}</p>
    </div>
</body>
</html>";

}