using BandR.Data;
using BandR.Entities;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class TokenService(ApplicationDbContext dbContext) : ITokenService
{
    public async Task CreateTokenAsync(Guid appUserId, byte[] tokenHash, DateTime expiresAt, CancellationToken ct)
    {
        var hashString = Convert.ToBase64String(tokenHash);
        var token = new RefreshToken
        {
            AppUserId = appUserId,
            TokenHash = hashString,
            ExpiresAt = expiresAt
        };
        dbContext.RefreshTokens.Add(token);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<RefreshToken?> GetTokenAsync(byte[] tokenHash, CancellationToken ct)
    {
        var hashString = Convert.ToBase64String(tokenHash);
        var token = await dbContext.RefreshTokens
            .Include(t => t.AppUser)
            .SingleOrDefaultAsync(t => t.TokenHash == hashString, ct);
        return token;
    }

    public async Task RemoveTokenAsync(byte[] tokenHash, CancellationToken ct)
    {
        var hashString = Convert.ToBase64String(tokenHash);
        var token = await dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == hashString, ct);
        if (token == null)
        {
            return;
        }

        dbContext.RefreshTokens.Remove(token);
        await dbContext.SaveChangesAsync(ct);
    }
}
