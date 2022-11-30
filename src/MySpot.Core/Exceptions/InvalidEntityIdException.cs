namespace MySpot.Core.Exceptions;

public sealed class InvalidEntityIdException : CustomException
{
    public InvalidEntityIdException(Guid value)
        : base($"Entity ID '{value}', is invalid.")
    {
    }
}