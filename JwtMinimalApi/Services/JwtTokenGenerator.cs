using JwtMinimalApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtMinimalApi.Services;

public class JwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserService _userService;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, UserService userService)
    {
        _jwtSettings = jwtSettings.Value;
        _userService = userService;
    }

    public string? GenerateToken(User inputUser)
    {
        var user = _userService.GetValidUser(inputUser);
        if (user == null)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, inputUser.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

}
