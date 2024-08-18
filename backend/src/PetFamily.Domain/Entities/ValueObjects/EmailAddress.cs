namespace PetFamily.Domain.Entities.ValueObjects;

public record EmailAddress
{
    private EmailAddress(string value)
    {
        Value = value;
    }
    
    public string Value { get; private set; }

    public static EmailAddress Create(string value)
    {
        var email = new EmailAddress(value);

        return email;
    }
}