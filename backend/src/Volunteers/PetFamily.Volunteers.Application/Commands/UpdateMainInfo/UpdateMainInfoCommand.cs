using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs.Shared;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description) : ICommand;