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


}