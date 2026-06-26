namespace BandR.DTOs.Account;

public record LoginDto(
    string Email,
    string Password
);