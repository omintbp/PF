using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Commands.TakeVolunteerRequestToReview;

public record TakeVolunteerRequestToReviewCommand(
    Guid VolunteerRequestId,
    Guid AdminId) : ICommand;