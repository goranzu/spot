using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record JobTitle
{
    public string Value { get; }
    public const string Employee = nameof(Employee);
    public const string Manager = nameof(Manager);
    public const string Boss = nameof(Boss);

    // public JobTitle(string value)
    // {
    //     if (string.IsNullOrEmpty(value) || (value != Employee && value != Manager || value != Boss))
    //     {
    //         throw new InvalidJobTitleException(value);
    //     }
    //
    //     Value = value;
    // }

    public static implicit operator string(JobTitle jobTitle)
        => jobTitle.Value;
    
    public static implicit operator JobTitle(string jobTitle)
        => new(jobTitle);
};