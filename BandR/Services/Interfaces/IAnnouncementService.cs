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

    public Task<AnnouncementDto> CreateAnnouncement(CreateAnnouncementDto announcement, Guid musicianId,
        CancellationToken cancellationToken);

    public Task<AnnouncementDto> UpdateAnnouncement(
        Guid announcementId,
        Guid musicianId,
        UpdateAnnouncementDto dto,
        CancellationToken cancellationToken);

    public Task DeleteAnnouncement(Guid id, Guid musicianId, CancellationToken cancellationToken);
}