using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BandR.Configuration;
using BandR.DTOs.Account;
using BandR.Entities;
using BandR.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace BandR.Services;

public class JwtService(
    IOptions<JwtConfiguration> jwtConfiguration,
    ITokenService tokenService
) : IJwtService
{
    private readonly JwtConfiguration JwtConfiguration = jwtConfiguration.Value;

    public async Task<AuthTokenResult> CreateAuthTokenAsync(ApplicationUser user, CancellationToken ct)
    {
        return await CreateFullAuthTokenAsync(user, ct);
    }

    public async Task<AuthTokenResult?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        byte[] tokenBytes;
        try
        {
            tokenBytes = Convert.FromBase64String(refreshToken);
        }
        catch
        {
            return null;
        }

        byte[] incomingHash = HashToken(tokenBytes);

        var token = await tokenService.GetTokenAsync(incomingHash, cancellationToken);

        if (token is null)
        {
            return null;
        }

        await tokenService.RemoveTokenAsync(incomingHash, cancellationToken);
        if (token.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        return await CreateFullAuthTokenAsync(token.AppUser, cancellationToken);
    }

    public async Task RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        byte[] incomingBytes;
        try
        {
            incomingBytes = Convert.FromBase64String(refreshToken);
        }
        catch
        {
            return;
        }

        await tokenService.RemoveTokenAsync(HashToken(incomingBytes), cancellationToken);
    }

    private async Task<AuthTokenResult> CreateFullAuthTokenAsync(ApplicationUser user, CancellationToken ct)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(JwtConfiguration.ExpirationInMinutes);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.SecretKey));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            JwtConfiguration.Issuer,
            JwtConfiguration.Audience,
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(tokenGenerator);

        Byte[] tokenBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }

        byte[] tokenHash = HashToken(tokenBytes);

        DateTime refreshTokenExpiration = DateTime.UtcNow.AddDays(JwtConfiguration.RefreshExpiryDays);
        await tokenService.CreateTokenAsync(user.Id, tokenHash, refreshTokenExpiration, ct);

        string refreshToken = Convert.ToBase64String(tokenBytes);

        return new AuthTokenResult(token, refreshToken, JwtConfiguration.ExpirationInMinutes * 60);
    }

    private byte[] HashToken(byte[] tokenData)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(tokenData);
    }
}
