using BandR.DTOs.Location;

namespace BandR.DTOs.Musicians;

public record MusicianDto(
    Guid Id,
    string Username,
    string City,
    List<string> Instruments,
    List<string> Tags,
    List<string> Styles,
    string? Bio,
    string? AvatarUrl
);