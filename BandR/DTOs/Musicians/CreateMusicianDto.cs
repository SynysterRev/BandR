namespace BandR.DTOs.Musicians;

public record CreateMusicianDto(
    string Username,
    string City,
    List<Guid>? InstrumentIds,
    List<Guid>? TagIds,
    List<Guid>? StyleIds,
    string? Bio
);