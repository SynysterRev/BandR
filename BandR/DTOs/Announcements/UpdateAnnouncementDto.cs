namespace BandR.DTOs.Announcements;

public record UpdateAnnouncementDto(
    string? Title,
    string? Description,
    string? City,
    List<Guid>? InstrumentIds,
    List<Guid>? TagIds,
    List<Guid>? StyleIds,
    bool? IsActive
);