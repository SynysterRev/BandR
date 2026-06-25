namespace BandR.DTOs.Account;

public record AuthTokenResult(
    string AccessToken,
    string RefreshToken,
    long ExpiresAt
);