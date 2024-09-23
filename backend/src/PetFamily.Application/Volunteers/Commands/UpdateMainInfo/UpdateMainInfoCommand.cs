using PetFamily.Application.Abstractions;
using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description) : ICommand;