namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";

            return Error.Validation($"value.is.invalid", $"{label} is invalid");
        }
    }
}