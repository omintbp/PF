namespace PetFamily.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";

            return Error.Validation($"value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }
        
        public static Error AlreadyExist(string? name)
        {
            var label = name ?? "record";
            
            return Error.Validation("record.already.exist", $"{label} already exist");
        }
    }

    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Your credentials is invalid");
        }

        public static Error RefreshTokenExpired()
        {
            return Error.Validation("refresh.token.expired", "Refresh token has expired");
        }
        
        public static Error TokenInvalid()
        {
            return Error.Validation("invalid.token", "Access token is anvalid");
        }
    }
}