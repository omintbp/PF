using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);

    Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);
    
    Task<Result<IReadOnlyList<Claim>, Error>> GetClaims(string jwtToken, CancellationToken cancellationToken = default);
}