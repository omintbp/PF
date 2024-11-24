using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequests.Application.Commands.RejectVolunteerRequest;

public record RejectVolunteerRequestCommand(
    Guid AdminId,
    Guid VolunteerRequestId,
    string RejectionComment) : ICommand;