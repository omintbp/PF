using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.IDs;
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
        VolunteerInfo volunteerInfo)
        : base(id)
    {
        UserId = userId;
        VolunteerInfo = volunteerInfo;
        CreatedAt = DateTime.UtcNow;
        Status = VolunteerRequestStatus.Submitted;
        RejectionComment = RejectionComment.None;
    }

    public Guid AdminId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid DiscussionId { get; private set; }

    public VolunteerInfo VolunteerInfo { get; private set; }

    public RejectionComment RejectionComment { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public VolunteerRequestStatus Status { get; private set; }

    public static Result<VolunteerRequest, Error> Create(
        VolunteerRequestId id,
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        if (userId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(userId));

        return new VolunteerRequest(
            id,
            userId,
            volunteerInfo);
    }

    public UnitResult<Error> TakeToReview(Guid adminId, Guid discussionId)
    {
        if (adminId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(adminId));

        if (discussionId == Guid.Empty)
            return Errors.General.ValueIsInvalid(nameof(discussionId));

        if (Status != VolunteerRequestStatus.Submitted && Status != VolunteerRequestStatus.RevisionRequired)
            return Errors.General.ValueIsInvalid(nameof(Status));

        AdminId = adminId;
        DiscussionId = discussionId;
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
        if (Status != VolunteerRequestStatus.OnReview)
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

    public UnitResult<Error> Update(VolunteerInfo volunteerInfo)
    {
        if (Status != VolunteerRequestStatus.RevisionRequired)
            return Errors.General.ValueIsInvalid(nameof(Status));

        VolunteerInfo = volunteerInfo;

        return UnitResult.Success<Error>();
    }
}