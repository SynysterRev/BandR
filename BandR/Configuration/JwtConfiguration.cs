namespace BandR.Configuration;

public record JwtConfiguration(
    string Issuer,
    string Audience,
    string ExpirationInMinutes,
    string SecretKey
);