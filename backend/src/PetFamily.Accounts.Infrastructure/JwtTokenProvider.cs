using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Models;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Core.Models;
using PetFamily.Core.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AuthorizationDbContext _dbContext;
    private readonly JwtOptions _options;

    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        AuthorizationDbContext dbContext)
    {
        _dbContext = dbContext;
        _options = options.Value;
    }

    public JwtTokenResult GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jti = Guid.NewGuid();

        List<Claim> claims =
        [
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")
        ];

        var configuredToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_options.ExpiredMinutesTime)),
            signingCredentials: signingCredentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(configuredToken);

        return new JwtTokenResult(jwtToken, jti);
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession()
        {
            User = user,
            Jti = jti,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(int.Parse(_options.RefreshDaysTime)),
            RefreshToken = Guid.NewGuid()
        };

        _dbContext.RefreshSessions.Add(refreshSession);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }

    public async Task<Result<IReadOnlyList<Claim>, Error>> GetClaims(
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        var handler = new JwtSecurityTokenHandler();
        var validationParams = TokenValidationParametersFactory.CreateWithoutLifeTime(_options);

        var validationTokenResult = await handler.ValidateTokenAsync(jwtToken, validationParams);
        if (validationTokenResult.IsValid == false)
            return Errors.User.TokenInvalid();

        return validationTokenResult.ClaimsIdentity.Claims.ToList();
    }
}