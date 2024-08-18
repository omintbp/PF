using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.ValueObjects;

public record EmailAddress
{
    private static readonly Regex ValidationRegex = new Regex(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + 
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
    
    private EmailAddress(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<EmailAddress, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value) || !ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(EmailAddress));
        
        var email = new EmailAddress(value);

        return email;
    }
}