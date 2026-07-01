using BandR.Common;
using BandR.DTOs.Announcements;
using BandR.Entities;

namespace BandR.Services.Interfaces;

public interface IAnnouncementService
{
    public Task<AnnouncementDto> GetAnnouncementByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsAsync(AnnouncementQueryFilter filter,
        CancellationToken cancellationToken);

    public Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsForMusicianAsync(Guid musicianId,
        AnnouncementQueryFilter filter, CancellationToken cancellationToken);

    public Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto announcement, Guid musicianId,
        CancellationToken cancellationToken);

    public Task<AnnouncementDto> UpdateAnnouncementAsync(
        Guid announcementId,
        Guid musicianId,
        UpdateAnnouncementDto dto,
        CancellationToken cancellationToken);

    public Task DeleteAnnouncementAsync(Guid id, Guid musicianId, CancellationToken cancellationToken);
}