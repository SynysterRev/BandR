using BandR.Entities;

namespace BandR.DTOs.Announcements;

public record UpdateAnnouncementDto(
    string? Title,
    string? Description,
    string? City,
    AnnouncementType? Type,
    List<Guid>? InstrumentIds,
    List<Guid>? TagIds,
    List<Guid>? StyleIds,
    bool? IsActive
);  