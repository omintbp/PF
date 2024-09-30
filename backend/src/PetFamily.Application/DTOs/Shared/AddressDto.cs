namespace PetFamily.Application.DTOs.Shared;

public class AddressDto
{
    public string Country { get; init; }
    public string City { get; init; }
    public string Street { get; init; } 
    public string House { get; init; }
    public string? Flat { get; init; }
}