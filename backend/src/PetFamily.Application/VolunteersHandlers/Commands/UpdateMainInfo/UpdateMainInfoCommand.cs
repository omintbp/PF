using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.VolunteersHandlers.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description) : ICommand;