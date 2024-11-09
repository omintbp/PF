namespace PetFamily.Accounts.Application.Models;

public record JwtTokenResult(string Token, Guid Jti);