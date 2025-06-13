using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SprintInventory.Core.Interfaces.Services.Security;
using SprintInventory.Core.Models.DTOs;
using SprintInventory.Core.Models.Settings;

namespace SprintInventory.Services.Security;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;
    
    public JwtService(JwtSettings settings)
    {
        _settings = settings;
    }
    
    public Result<string> GenerateToken(Guid userId)
    {
        try
        {
            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.NameId, userId.ToString())
            };
            
            var encodedKey = Encoding.UTF8.GetBytes(_settings.Secret);
            var key = new SymmetricSecurityKey(encodedKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                audience: _settings.Audience,
                issuer: _settings.Issuer
            );
            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception e)
        {
            return Result<string>.Failure($"Error generating token: {e.Message}");
        }
    }
}