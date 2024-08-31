using PetFamily.Application.SharedDTOs;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoDto(
    FullNameDto FullName,
    string PhoneNumber,
    int Experience,
    string Description);