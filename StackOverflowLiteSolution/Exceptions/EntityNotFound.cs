namespace Stackoverflow_Lite.Exceptions;

public class EntityNotFound : Exception
{
    public EntityNotFound (string message) : base(message){}
}