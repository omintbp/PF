using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Application.Commands.UpdateVolunteerRequest;

public record UpdateVolunteerRequestCommand(
    Guid VolunteerRequestId,
    Guid UserId,
    int Experience,
    IEnumerable<RequisiteDto> Requisites) : ICommand;