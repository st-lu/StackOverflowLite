namespace Stackoverflow_Lite.Exceptions;

public class OidcUserMappingAlreadyCreated : Exception
{
    public OidcUserMappingAlreadyCreated(string? message) : base(message)
    {
    }
}