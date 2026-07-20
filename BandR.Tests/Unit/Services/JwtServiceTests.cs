using System.Security.Cryptography;
using BandR.Configuration;
using BandR.Services;
using BandR.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace BandR.Tests.Unit.Services;

public class JwtServiceTests
{
    [Fact]
    public async Task RevokeTokenAsync_ShouldRemoveHashedToken_WhenRefreshTokenIsValidBase64()
    {
        var tokenBytes = new byte[] { 1, 2, 3, 4 };
        var refreshToken = Convert.ToBase64String(tokenBytes);
        var expectedHash = SHA256.HashData(tokenBytes);
        var tokenService = new Mock<ITokenService>();
        var jwtService = CreateJwtService(tokenService.Object);

        await jwtService.RevokeTokenAsync(refreshToken, CancellationToken.None);

        tokenService.Verify(
            service => service.RemoveTokenAsync(
                It.Is<byte[]>(hash => hash.SequenceEqual(expectedHash)),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task RevokeTokenAsync_ShouldNotRemoveToken_WhenRefreshTokenIsInvalid()
    {
        var tokenService = new Mock<ITokenService>();
        var jwtService = CreateJwtService(tokenService.Object);

        await jwtService.RevokeTokenAsync("not-base64", CancellationToken.None);

        tokenService.Verify(
            service => service.RemoveTokenAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldRejectToken_WhenAccountIsDeactivated()
    {
        var tokenBytes = new byte[] { 1, 2, 3, 4 };
        var refreshToken = Convert.ToBase64String(tokenBytes);
        var tokenService = new Mock<ITokenService>();
        tokenService
            .Setup(service => service.GetTokenAsync(It.IsAny<byte[]>(), CancellationToken.None))
            .ReturnsAsync(new BandR.Entities.RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                AppUser = new BandR.Entities.ApplicationUser { DeactivatedAt = DateTime.UtcNow }
            });
        var jwtService = CreateJwtService(tokenService.Object);

        var result = await jwtService.RefreshTokenAsync(refreshToken, CancellationToken.None);

        result.Should().BeNull();
        tokenService.Verify(service => service.CreateTokenAsync(
            It.IsAny<Guid>(), It.IsAny<byte[]>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private static JwtService CreateJwtService(ITokenService tokenService)
    {
        var configuration = Options.Create(new JwtConfiguration
        {
            SecretKey = "test-secret-key-with-a-safe-length",
            Issuer = "test-issuer",
            Audience = "test-audience"
        });

        return new JwtService(configuration, tokenService);
    }
}
