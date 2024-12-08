namespace Stackoverflow_Lite.Exceptions;

public class OperationNotAllowed : Exception
{
    public OperationNotAllowed(string message) : base(message)
    {
    }
}