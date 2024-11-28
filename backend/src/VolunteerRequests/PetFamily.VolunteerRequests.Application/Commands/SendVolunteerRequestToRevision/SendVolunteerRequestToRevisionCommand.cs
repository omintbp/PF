using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Commands.SendVolunteerRequestToRevision;

public record SendVolunteerRequestToRevisionCommand(
    Guid AdminId,
    Guid VolunteerRequestId, 
    string RejectionComment) : ICommand;