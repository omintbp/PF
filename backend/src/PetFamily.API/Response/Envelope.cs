using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record Envelope
{
    public object? Result { get; }

    public ErrorList? Errors { get; }

    public DateTime GeneratedAt { get; }

    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        GeneratedAt = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new Envelope(result, null);

    public static Envelope Error(ErrorList errors) =>
        new Envelope(null, errors);
}