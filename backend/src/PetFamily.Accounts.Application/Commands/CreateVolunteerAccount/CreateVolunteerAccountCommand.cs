using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Accounts.Application.Commands.CreateVolunteerAccount;

public record CreateVolunteerAccountCommand(
    Guid UserId,
    int Experience,
    IEnumerable<RequisiteDto> Requisites) : ICommand;