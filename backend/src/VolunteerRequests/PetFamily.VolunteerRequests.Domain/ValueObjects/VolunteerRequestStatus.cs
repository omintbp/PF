using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerRequests.Domain.ValueObjects;

public class VolunteerRequestStatus : ValueObject
{
    public static VolunteerRequestStatus Submitted => new(nameof(Submitted));

    public static VolunteerRequestStatus Rejected => new(nameof(Rejected));

    public static VolunteerRequestStatus RevisionRequired => new(nameof(RevisionRequired));

    public static VolunteerRequestStatus Approved => new(nameof(Approved));

    public static VolunteerRequestStatus OnReview => new(nameof(OnReview));

    private static readonly List<VolunteerRequestStatus> _statuses =
    [
        Submitted,
        Rejected,
        RevisionRequired,
        Approved,
        OnReview
    ];

    private VolunteerRequestStatus()
    {
    }

    private VolunteerRequestStatus(string status)
    {
        Status = status;
    }

    public string Status { get; }

    public Result<VolunteerRequestStatus, Error> Create(string status)
    {
        var isStatusAllowed = _statuses.Any(s =>
            string.Equals(s.Status, status, StringComparison.OrdinalIgnoreCase));

        if (isStatusAllowed == false)
            return Errors.General.ValueIsInvalid(nameof(status));

        return new VolunteerRequestStatus(status);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Status;
    }
}