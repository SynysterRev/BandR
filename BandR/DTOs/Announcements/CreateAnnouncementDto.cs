using BandR.Entities;

namespace BandR.DTOs.Announcements;

public record CreateAnnouncementDto(
    string Title,
    string Description,
    string City,
    AnnouncementType Type,
    List<Guid> InstrumentIds,
    List<Guid> TagIds,
    List<Guid> StyleIds
);