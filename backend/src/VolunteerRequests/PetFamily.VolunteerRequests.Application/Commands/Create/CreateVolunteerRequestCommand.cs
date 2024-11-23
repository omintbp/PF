using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.VolunteerRequests.Application.Commands.Create;

public record CreateVolunteerRequestCommand(
    Guid UserId,
    int Experience,
    IEnumerable<RequisiteDto> Requisites) : ICommand;