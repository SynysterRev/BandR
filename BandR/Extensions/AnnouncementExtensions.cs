using BandR.DTOs.Announcements;
using BandR.Entities;

namespace BandR.Extensions;

public static class AnnouncementExtensions
{
    public static Announcement ToEntity(this CreateAnnouncementDto dto, Guid musicianId, Location location)
    {
        return new Announcement
        {
            Title = dto.Title,
            Description = dto.Description,
            MusicianId = musicianId,
            Location = location,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static AnnouncementDto ToDto(this Announcement announcement)
    {
        return new AnnouncementDto(
            announcement.Id,
            announcement.Title,
            announcement.Description,
            announcement.Location.City,
            announcement.Musician.Id,
            announcement.Musician.Username,
            announcement.Type,
            announcement.AnnouncementInstruments.Select(ai => ai.Instrument.Name).ToList(),
            announcement.Tags.Select(t => t.Name).ToList(),
            announcement.Styles.Select(s => s.Name).ToList(),
            announcement.CreatedAt
        );
    }

    public static AnnouncementListDto ToListDto(this Announcement announcement)
    {
        return new AnnouncementListDto(
            announcement.Id,
            announcement.Title,
            announcement.Location.City,
            announcement.Type,
            announcement.Musician.Id,
            announcement.Musician.Username,
            announcement.AnnouncementInstruments.Select(ai => ai.Instrument.Name).ToList(),
            announcement.Styles.Select(s => s.Name).ToList(),
            announcement.CreatedAt
        );
    }
}