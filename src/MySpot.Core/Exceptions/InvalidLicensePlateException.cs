namespace MySpot.Core.Exceptions;

public class InvalidLicensePlateException : CustomException
{
    public InvalidLicensePlateException(string message) 
        : base($"License plate cannot be empty.")
    {
    }
}