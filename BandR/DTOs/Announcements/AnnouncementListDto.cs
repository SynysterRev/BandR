using BandR.Entities;

namespace BandR.DTOs.Announcements;

public record AnnouncementListDto(
    Guid Id,
    string Title,
    string City,
    AnnouncementType Type,
    Guid MusicianId,
    string MusicianUsername,
    List<string> Instruments,
    List<string> Styles,
    DateTime CreatedAt
);