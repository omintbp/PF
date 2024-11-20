using PetFamily.VolunteerRequests.Domain.AggregateRoot;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using FluentAssertions;
using PetFamily.VolunteerRequests.Domain.Enums;

namespace PetFamily.VolunteerRequests.Domain.UnitTests;

public class VolunteerRequestsTests
{
    [Fact]
    public void Take_Request_To_Review_Should_Return_Success_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        var result = volunteerRequest.TakeToReview(adminId);

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.OnReview);
        volunteerRequest.AdminId.Should().Be(adminId);
    }

    [Fact]
    public void Take_Request_To_Review_Should_Return_Error_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        volunteerRequest.TakeToReview(adminId);
        volunteerRequest.Reject(rejectionComment);

        var result = volunteerRequest.TakeToReview(adminId);

        // assert
        result.IsSuccess.Should().BeFalse();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Rejected);
    }

    [Fact]
    public void Reject_Request_Should_Return_Success_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        var takeToReviewResult = volunteerRequest.TakeToReview(adminId);
        var rejectResult = volunteerRequest.Reject(rejectionComment);

        // assert
        takeToReviewResult.IsSuccess.Should().BeTrue();
        rejectResult.IsSuccess.Should().BeTrue();

        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Rejected);
        volunteerRequest.AdminId.Should().Be(adminId);
        volunteerRequest.RejectionComment.Should().Be(rejectionComment);
    }

    [Fact]
    public void Reject_Request_Should_Return_Error_Result_If_Current_Status_Not_Valid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        var rejectResult = volunteerRequest.Reject(rejectionComment);

        // assert
        rejectResult.IsSuccess.Should().BeFalse();

        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Submitted);
        volunteerRequest.RejectionComment.Should().NotBe(rejectionComment);
    }

    [Fact]
    public void Approve_Request_Should_Return_Success_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        var result = volunteerRequest.TakeToReview(adminId);
        volunteerRequest.Approve();

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Approved);
    }

    [Fact]
    public void Approve_Request_Should_Return_Error_Result_If_Status_Not_Valid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        volunteerRequest.TakeToReview(adminId);
        volunteerRequest.Reject(rejectionComment);

        var result = volunteerRequest.Approve();

        // assert
        result.IsSuccess.Should().BeFalse();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Rejected);
    }

    [Fact]
    public void Send_Request_To_Revision_Should_Return_Success_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        volunteerRequest.TakeToReview(adminId);
        var result = volunteerRequest.SendToRevision(rejectionComment);

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.RevisionRequired);
        volunteerRequest.AdminId.Should().Be(adminId);
        volunteerRequest.RejectionComment.Should().Be(rejectionComment);
    }

    [Fact]
    public void Send_Request_To_Review_After_Revision_Should_Return_Success_Result()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var secondAdminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        volunteerRequest.TakeToReview(adminId);
        volunteerRequest.SendToRevision(rejectionComment);

        var result = volunteerRequest.TakeToReview(secondAdminId);

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.OnReview);
        volunteerRequest.AdminId.Should().Be(secondAdminId);
        volunteerRequest.RejectionComment.Should().Be(rejectionComment);
    }

    [Fact]
    public void Send_Request_To_Revision_Should_Return_Error_Result_If_Status_Not_Valid()
    {
        // arrange
        var userId = Guid.NewGuid();
        var discussionId = Guid.NewGuid();
        var volunteerInfo = VolunteerInfo.Create("Volunteer info").Value;
        var adminId = Guid.NewGuid();
        var rejectionComment = RejectionComment.Create("Rejection comment").Value;
        var secondRejectionComment = RejectionComment.Create("Second rejection comment").Value;

        var volunteerRequest = VolunteerRequest.Create(
            userId,
            discussionId,
            volunteerInfo).Value;

        // act
        volunteerRequest.TakeToReview(adminId);
        volunteerRequest.Reject(rejectionComment);

        var result = volunteerRequest.SendToRevision(secondRejectionComment);

        // assert
        result.IsSuccess.Should().BeFalse();
        volunteerRequest.Status.Should().Be(VolunteerRequestStatus.Rejected);
        volunteerRequest.RejectionComment.Should().NotBe(secondRejectionComment);
    }
}