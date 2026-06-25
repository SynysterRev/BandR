namespace BandR.DTOs.Account;

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);