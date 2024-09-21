using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description);