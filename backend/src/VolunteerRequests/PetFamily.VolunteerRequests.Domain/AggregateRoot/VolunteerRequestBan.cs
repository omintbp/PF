using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.VolunteerRequests.Domain.AggregateRoot;

public class VolunteerRequestBan : SharedKernel.Entity<VolunteerRequestBanId>
{
    private VolunteerRequestBan(VolunteerRequestBanId id)
        : base(id)
    {
    }

    private VolunteerRequestBan(
        VolunteerRequestBanId id,
        Guid userId,
        DateTime banDate,
        DateTime expiryDate)
        : base(id)
    {
        UserId = userId;
        BanDate = banDate;
        ExpiryDate = expiryDate;
    }

    public Guid UserId { get; private set; }

    public DateTime BanDate { get; private set; }

    public DateTime ExpiryDate { get; private set; }

    public static Result<VolunteerRequestBan, Error> Create(
        VolunteerRequestBanId id,
        Guid userId,
        DateTime banDate,
        DateTime expiryDate)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(UserId));

        if (expiryDate < banDate)
            return Errors.General.ValueIsInvalid(nameof(ExpiryDate));

        return new VolunteerRequestBan(id, userId, banDate, expiryDate);
    }

    public bool IsExpiredFor(DateTime date) => date < ExpiryDate;

    public bool IsActiveFor(DateTime date) => date >= ExpiryDate;
}