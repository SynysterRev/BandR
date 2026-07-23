namespace BandR.DTOs.Account;

public record AccessTokenResult(
    string AccessToken,
    long ExpiresAt
);
