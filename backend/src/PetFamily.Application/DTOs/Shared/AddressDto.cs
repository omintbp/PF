namespace PetFamily.Application.DTOs.Shared;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string House,
    string? Flat);