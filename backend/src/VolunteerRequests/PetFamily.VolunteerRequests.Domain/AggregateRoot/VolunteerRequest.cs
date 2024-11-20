using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
using PetFamily.VolunteerRequests.Domain.Enums;
using PetFamily.VolunteerRequests.Domain.ValueObjects;

namespace PetFamily.VolunteerRequests.Domain.AggregateRoot;

public class VolunteerRequest : SharedKernel.Entity<VolunteerRequestId>
{
    private VolunteerRequest()
    {
    }

    private VolunteerRequest(
        VolunteerRequestId id,
        Guid userId,
        Guid discussionId,
        VolunteerInfo volunteerInfo)
        : base(id)
    {
        UserId = userId;
        DiscussionId = discussionId;
        VolunteerInfo = volunteerInfo;
        CreatedAt = DateTime.UtcNow;
        Status = VolunteerRequestStatus.Submitted;
    }

    public Guid AdminId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid DiscussionId { get; private set; }

    public VolunteerInfo VolunteerInfo { get; private set; }

    public RejectionComment RejectionComment { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public VolunteerRequestStatus Status { get; private set; }

    public static Result<VolunteerRequest, Error> Create(
        Guid userId,
        Guid discussionId,
        VolunteerInfo volunteerInfo)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(userId));

        if (discussionId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(discussionId));

        var id = VolunteerRequestId.NewVolunteerRequestId();

        return new VolunteerRequest(
            id,
            userId,
            discussionId,
            volunteerInfo);
    }

    public UnitResult<Error> TakeToReview(Guid adminId)
    {
        if (adminId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(adminId));

        if (Status != VolunteerRequestStatus.Submitted && Status != VolunteerRequestStatus.RevisionRequired)
            return Errors.General.ValueIsInvalid(nameof(Status));

        AdminId = adminId;
        Status = VolunteerRequestStatus.OnReview;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> SendToRevision(RejectionComment comment)
    {
        if (Status != VolunteerRequestStatus.OnReview)
            return Errors.General.ValueIsInvalid(nameof(Status));

        Status = VolunteerRequestStatus.RevisionRequired;
        RejectionComment = comment;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Reject(RejectionComment comment)
    {
        if(Status != VolunteerRequestStatus.OnReview)
            return Errors.General.ValueIsInvalid(nameof(Status));
            
        Status = VolunteerRequestStatus.Rejected;
        RejectionComment = comment;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Approve()
    {
        if (Status != VolunteerRequestStatus.OnReview)
            return Errors.General.ValueIsInvalid(nameof(Status));

        Status = VolunteerRequestStatus.Approved;

        return UnitResult.Success<Error>();
    }
}