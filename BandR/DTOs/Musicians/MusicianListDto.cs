namespace BandR.DTOs.Musicians;

public record MusicianListDto(
    Guid Id,
    string Username,
    string Location
);