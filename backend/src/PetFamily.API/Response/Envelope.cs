namespace PetFamily.API.Response;

public record Envelope
{
    public object? Result { get; }

    public List<ResponseError> Errors { get; }

    public DateTime GeneratedAt { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        GeneratedAt = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new Envelope(result, []);

    public static Envelope Error(IEnumerable<ResponseError> errors) =>
        new Envelope(null, errors);
}