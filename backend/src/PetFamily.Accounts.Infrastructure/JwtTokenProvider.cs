using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Options;
using PetFamily.Species.Application;

namespace PetFamily.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        List<Claim> claims = 
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        ];
        
        var configuredToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_options.ExpiredMinutesTime)),
            signingCredentials: signingCredentials);
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(configuredToken);

        return jwtToken;
    }
}