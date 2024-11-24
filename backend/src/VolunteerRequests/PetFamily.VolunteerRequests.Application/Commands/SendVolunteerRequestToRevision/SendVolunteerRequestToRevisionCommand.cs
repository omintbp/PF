using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Commands.SendVolunteerRequestToRevision;

public record SendVolunteerRequestToRevisionCommand(
    Guid VolunteerRequestId, 
    string RejectionComment) : ICommand;