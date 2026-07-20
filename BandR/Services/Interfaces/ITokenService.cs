using BandR.Entities;

namespace BandR.Services.Interfaces;

public interface ITokenService
{
    Task CreateTokenAsync(Guid appUserId, byte[] tokenHash, DateTime expiresAt, CancellationToken ct);
    Task<RefreshToken?> GetTokenAsync(byte[] tokenHash, CancellationToken ct);
    Task RemoveTokenAsync(byte[] tokenHash, CancellationToken ct);
    Task RemoveTokensForUserAsync(Guid appUserId, CancellationToken ct);
}
