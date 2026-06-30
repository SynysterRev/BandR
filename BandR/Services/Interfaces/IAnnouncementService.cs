using BandR.Common;
using BandR.DTOs.Announcements;
using BandR.Entities;

namespace BandR.Services.Interfaces;

public interface IAnnouncementService
{
    public Task<AnnouncementDto> GetAnnouncementById(Guid id, CancellationToken cancellationToken);

    public Task<PagedResponse<AnnouncementListDto>> GetAnnouncements(AnnouncementQueryFilter filter,
        CancellationToken cancellationToken);

    public Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsForMusician(Guid musicianId,
        AnnouncementQueryFilter filter, CancellationToken cancellationToken);

    public Task<AnnouncementDto> CreateAnnouncement(CreateAnnouncementDto announcement,
        CancellationToken cancellationToken);

    public Task DeleteAnnouncement(Announcement announcement, CancellationToken cancellationToken);
}