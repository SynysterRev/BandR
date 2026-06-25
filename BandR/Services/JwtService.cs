using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BandR.Configuration;
using BandR.DTOs.Account;
using BandR.Entities;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
// using Razing.Commons.AspNetCore;
// using Razing.Hyve.Authentication.Tokens;
// using Razing.Hyve.PlayerData.Models;

// namespace Razing.Hyve.Authentication.Internal;

public class JwtService : IJwtService
{
    // private readonly IMongoTokenStore m_TokenStore;
    private readonly JwtConfiguration JwtConfiguration;

    public JwtService(IOptions<JwtConfiguration> jwtConfiguration)
    {
        // m_TokenStore = tokenStore;
        JwtConfiguration = jwtConfiguration.Value;
    }

    // public async Task<AuthTokenResult> CreateAuthTokenAsync(ApplicationUser user)
    // {
    //     var principal = MapToPrincipal(player);
    //     return await CreateFullAuthTokenAsync(principal);
    // }

    public async Task<AuthTokenResult?> RefreshTokenAsync(string refreshToken)
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

        var tokenData = await m_TokenStore.GetTokenAsync(incomingHash);

        if (tokenData is null)
        {
            return null;
        }

        var authTicket = TicketSerializer.Default.Deserialize(tokenData);

        if (authTicket is null)
        {
            return null;
        }

        await m_TokenStore.RemoveTokenAsync(incomingHash);
        if (authTicket.Properties.ExpiresUtc is null || authTicket.Properties.ExpiresUtc <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        return await CreateFullAuthTokenAsync(authTicket.Principal);
    }

    public async Task RevokeTokenAsync(string refreshToken)
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
        await m_TokenStore.RemoveTokenAsync(incomingBytes);
    }

    private ClaimsPrincipal MapToPrincipal(PlayerDataModel player)
    {
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, player.Id),
            new Claim(ClaimTypes.NameIdentifier, player.Id),
            new Claim("username", player.Profile.DisplayName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(player.Authentication.SteamId))
            claims.Add(new Claim("steam_id", player.Authentication.SteamId));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
    }

    private async Task<AuthTokenResult> CreateFullAuthTokenAsync(ClaimsPrincipal claimsPrincipal)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(JwtConfiguration.ExpiryMinutes);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.Key));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            JwtConfiguration.Issuer,
            JwtConfiguration.Audience,
            claimsPrincipal.Claims,
            expires: expiration,
            signingCredentials: signingCredentials
            );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(tokenGenerator);

        var props = new AuthenticationProperties
        {
            IssuedUtc = DateTimeOffset.UtcNow.AddSeconds(-5),
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(JwtConfiguration.RefreshExpiryDays),
        };

        var ticket = new AuthenticationTicket(claimsPrincipal, props, JwtBearerDefaults.AuthenticationScheme);

        Byte[] tokenBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }

        byte[] tokenHash = HashToken(tokenBytes);

        await m_TokenStore.CreateTokenAsync(claimsPrincipal.GetUserId()!, ticket, tokenHash);

        string refreshToken = Convert.ToBase64String(tokenBytes);

        return new AuthTokenResult(token, refreshToken, JwtConfiguration.ExpiryMinutes * 60);
    }

    private byte[] HashToken(byte[] tokenData)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(tokenData);
    }
}
