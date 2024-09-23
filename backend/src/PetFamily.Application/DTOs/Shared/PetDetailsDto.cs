namespace PetFamily.Application.DTOs.Shared;

public record PetDetailsDto(
    double Weight,
    double Height,
    bool IsCastrated,
    bool IsVaccinated,
    string Color,
    string HealthInfo,
    DateTime Birthday);