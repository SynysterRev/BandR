using BandR.DTOs.Account;
using BandR.Entities;

namespace BandR.Services.Interfaces;

public interface IJwtService
{
    /// <summary>
    /// Create an access and a refresh token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task<AuthTokenResult> CreateAuthTokenAsync(ApplicationUser user, CancellationToken ct);

    /// <summary>
    /// Refresh a refresh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<AuthTokenResult?> RefreshTokenAsync(string refreshToken,  CancellationToken ct);

    /// <summary>
    /// Revoke a refresh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task RevokeTokenAsync(string refreshToken, CancellationToken ct);
}