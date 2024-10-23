namespace PetFamily.Core.DTOs.Volunteers;

public class PetDetailsDto
{
    public double Weight { get; init; }
    public double Height { get; init; }
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public string Color { get; init; }
    public string HealthInfo { get; init; }
    public DateTime BirthdayDate { get; init; }
}