namespace PetFamily.Domain.Shared;

public record Error
{
    public const string SEPARATOR = "||";

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }
    
    public string? InvalidField { get; }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static Error Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public string Serialize() => string.Join(SEPARATOR, Code, Message, Type, InvalidField);

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 4)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var errorType) == false)
        {
            throw new ArgumentException("Invalid serialize format");
        }

        return new Error(parts[0], parts[1], errorType, parts[3]);
    }

    public ErrorList ToErrorList() => new([this]);
}