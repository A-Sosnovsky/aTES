using System;
using System.Collections.Generic;
using System.Linq;
using JwtRoleAuthentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JwtRoleAuthentication.Services;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private const int ExpirationMinutes = 60;
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public string CreateToken(ApplicationUser user, IList<string> roles)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user, roles),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        
        _logger.LogInformation("JWT Token created");
        
        return tokenHandler.WriteToken(token);
    }

    private static JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private static IEnumerable<Claim> CreateClaims(ApplicationUser user, IList<string> roles)
    {
        var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
        
        try
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, jwtSub),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
        
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}