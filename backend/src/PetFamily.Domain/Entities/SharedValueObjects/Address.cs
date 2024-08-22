using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.SharedValueObjects;

public record Address
{
    private Address(string country, string city, string street, string house, string flat)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }

    public string Country { get; }
    
    public string City { get; }
    
    public string Street { get; }
    
    public string House { get; }
    
    public string Flat { get; }

    public static Result<Address, Error> Create(string country, string city, string street, string house, string flat)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Errors.General.ValueIsInvalid(nameof(country));
        
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsInvalid(nameof(city));
        
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid(nameof(street));
        
        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsInvalid(nameof(house));
        
        var address = new Address(country, city, street, house, flat);
        
        return address;
    }
}