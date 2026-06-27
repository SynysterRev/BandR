using BandR.DTOs.Announcements;
using BandR.Entities;

namespace BandR.Services.Interfaces;

public interface IAnnouncementService
{
    public Task<AnnouncementDto> GetAnnouncementById(Guid id, CancellationToken cancellationToken);
    public Task<List<AnnouncementListDto>> GetAnnouncements(CancellationToken cancellationToken);
    public Task<List<AnnouncementListDto>> GetAnnouncementsForMusician(Guid musicianId, CancellationToken cancellationToken);
    public Task<AnnouncementDto> CreateAnnouncement(CreateAnnouncementDto announcement, CancellationToken cancellationToken);
    public Task DeleteAnnouncement(Announcement announcement, CancellationToken cancellationToken);
}