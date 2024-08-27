using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex ValidatePhoneNumberRegex = new(
        @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$");

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ValidatePhoneNumberRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));

        var phoneNumber = new PhoneNumber(value);

        return phoneNumber;
    }
}