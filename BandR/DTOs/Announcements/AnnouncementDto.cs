using BandR.Entities;

namespace BandR.DTOs.Announcements;

public record AnnouncementDto(
    Guid Id,
    string Title,
    string Description,
    string City,
    Guid MusicianId,
    string MusicianUsername,
    AnnouncementType Type,
    List<string> Instruments,
    List<string> Tags,
    List<string> Styles,
    DateTime CreatedAt
);