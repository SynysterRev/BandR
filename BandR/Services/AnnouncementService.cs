using BandR.Common;
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

    public async Task<PagedResponse<AnnouncementListDto>> GetAnnouncements(AnnouncementQueryFilter filter,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);
        
        var query = dbContext.Announcements.AsNoTracking().AsQueryable();
        
        var totalRecords = await query.CountAsync(cancellationToken);
        
        query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);
        
        var announcements = await query.ApplyPagination(pageNumber, pageSize)
            .Select(a => a.ToListDto())
            .ToListAsync(cancellationToken);

        return new PagedResponse<AnnouncementListDto>
        {
            Data = announcements,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }

    public Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsForMusician(
        Guid musicianId,
        AnnouncementQueryFilter filter,
        CancellationToken cancellationToken
    )
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