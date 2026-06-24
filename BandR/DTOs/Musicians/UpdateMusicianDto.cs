namespace BandR.DTOs.Musicians;

public record UpdateMusicianDto(
    string? Username,
    string? City,
    string? Bio,
    List<Guid>? InstrumentIds,
    List<Guid>? StyleIds,
    List<Guid>? TagIds
);