namespace PetFamily.Domain.Entities.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }
    
    public string Value { get; private set; }

    public static PhoneNumber Create(string value)
    {
        var phoneNumber = new PhoneNumber(value);

        return phoneNumber;
    }
}