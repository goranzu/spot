namespace MySpot.Core.Exceptions;

public sealed class InvalidJobTitleException : CustomException
{
    public string Value { get; }

    public InvalidJobTitleException(string value)
        : base($"Job title with value '{value}' is incorrect.")
    {
        Value = value;
    }
}