using BandR.Data;
using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class AnnouncementService(ApplicationDbContext dbContext) : IAnnouncementService
{
    public async Task<AnnouncementDto> GetAnnouncementById(Guid id, CancellationToken cancellationToken)
    {
        var announcement = await dbContext.Announcements
            .Include(a => a.Location)
            .Include(a => a.AnnouncementInstruments).ThenInclude(ai => ai.Instrument)
            .Include(a => a.Styles)
            .Include(a => a.Tags)
            .AsSplitQuery()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (announcement is null)
        {
            throw new AnnouncementException.AnnouncementNotFoundException(id);
        }

        return announcement.ToDto();
    }

    public Task<List<AnnouncementListDto>> GetAnnouncements(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<AnnouncementListDto>> GetAnnouncementsForMusician(Guid musicianId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AnnouncementDto> CreateAnnouncement(CreateAnnouncementDto announcement,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAnnouncement(Announcement announcement, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}