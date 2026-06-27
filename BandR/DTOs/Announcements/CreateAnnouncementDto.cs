namespace BandR.DTOs.Announcements;

public record CreateAnnouncementDto(
    string Title,
    string Description,
    string City,
    List<Guid> InstrumentIds,
    List<Guid> TagIds,
    List<Guid> StyleIds,
    DateTime CreatedAt
);