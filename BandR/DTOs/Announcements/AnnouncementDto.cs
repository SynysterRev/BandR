namespace BandR.DTOs.Announcements;

public record AnnouncementDto(
    Guid Id,
    string Title,
    string Description,
    string City,
    Guid MusicianId,
    string MusicianUsername,
    List<string> Instruments,
    List<string> Tags,
    List<string> Styles,
    DateTime CreatedAt
);