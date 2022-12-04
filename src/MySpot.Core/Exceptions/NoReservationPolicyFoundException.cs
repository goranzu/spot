namespace MySpot.Core.Exceptions;

public sealed class NoReservationPolicyFoundException : CustomException
{
    public string JobTitle { get; }

    public NoReservationPolicyFoundException(string jobTitle)
        : base($"No valid reservation policy found for job title '{jobTitle}'.")
    {
        JobTitle = jobTitle;
    }
}