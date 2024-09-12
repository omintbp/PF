namespace PetFamily.Application.SharedDTOs;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string House,
    string? Flat);