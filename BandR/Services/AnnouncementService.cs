using BandR.Common;
using BandR.Data;
using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Entities.Joints;
using BandR.Exceptions;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class AnnouncementService(ApplicationDbContext dbContext) : IAnnouncementService
{
    public async Task<AnnouncementDto> GetAnnouncementByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var announcement = await dbContext.Announcements
            .Include(a => a.Location)
            .Include(a => a.Musician)
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

    public async Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsAsync(AnnouncementQueryFilter filter,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = dbContext.Announcements.AsNoTracking()
            .AsQueryable()
            .Where(a => a.IsActive);
        
        query = query.ApplyFilters(filter);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt desc" : filter.SortBy);

        var announcements = await query.ApplyPagination(pageNumber, pageSize)
            .Select(a => new AnnouncementListDto(
                a.Id,
                a.Title,
                a.Location.City,
                a.Type,
                a.Musician.Id,
                a.Musician.Username,
                a.AnnouncementInstruments.Select(ai => ai.Instrument.Name).ToList(),
                a.Styles.Select(s => s.Name).ToList(),
                a.CreatedAt
            ))
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

    public async Task<PagedResponse<AnnouncementListDto>> GetAnnouncementsForMusicianAsync(
        Guid musicianId,
        AnnouncementQueryFilter filter,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = dbContext.Announcements.AsNoTracking()
            .AsQueryable()
            .Where(a => a.MusicianId == musicianId);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        var announcements = await query.ApplyPagination(pageNumber, pageSize)
            .Select(a => new AnnouncementListDto(
                a.Id,
                a.Title,
                a.Location.City,
                a.Type,
                a.Musician.Id,
                a.Musician.Username,
                a.AnnouncementInstruments.Select(ai => ai.Instrument.Name).ToList(),
                a.Styles.Select(s => s.Name).ToList(),
                a.CreatedAt
            ))
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

    public async Task<AnnouncementDto> CreateAnnouncementAsync(
        CreateAnnouncementDto announcementDto,
        Guid musicianId,
        CancellationToken cancellationToken
    )
    {
        var location = await dbContext.Locations
            .FirstOrDefaultAsync(l => l.City.ToLower() == announcementDto.City.ToLower(), cancellationToken);

        if (location is null)
        {
            location = new Location
            {
                City = announcementDto.City,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Locations.Add(location);
        }

        var announcement = announcementDto.ToEntity(musicianId, location);
        if (announcementDto.InstrumentIds is not null && announcementDto.InstrumentIds.Count > 0)
        {
            var instruments = await dbContext.Instruments
                .Where(i => announcementDto.InstrumentIds.Contains(i.Id))
                .ToListAsync(cancellationToken);
            announcement.AnnouncementInstruments = instruments.Select(i => new AnnouncementInstrument
            {
                Instrument = i
            }).ToList();
        }

        if (announcementDto.TagIds is not null && announcementDto.TagIds.Count > 0)
        {
            var tags = await dbContext.Tags
                .Where(t => announcementDto.TagIds.Contains(t.Id))
                .ToListAsync(cancellationToken);

            announcement.Tags = tags;
        }

        if (announcementDto.StyleIds is not null && announcementDto.StyleIds.Count > 0)
        {
            var styles = await dbContext.Styles
                .Where(s => announcementDto.StyleIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            announcement.Styles = styles;
        }

        await dbContext.Announcements.AddAsync(announcement, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return announcement.ToDto();
    }
    
    public async Task<AnnouncementDto> UpdateAnnouncementAsync(
    Guid announcementId,
    Guid musicianId,
    UpdateAnnouncementDto dto,
    CancellationToken cancellationToken)
{
    var announcement = await dbContext.Announcements
        .Include(a => a.Location)
        .Include(a => a.Tags)
        .Include(a => a.Styles)
        .Include(a => a.AnnouncementInstruments)
        .FirstOrDefaultAsync(a => a.Id == announcementId, cancellationToken);

    if (announcement is null)
    {
        throw new AnnouncementException.AnnouncementNotFoundException(announcementId);
    }

    if (announcement.MusicianId != musicianId)
    {
        throw new AnnouncementException.AnnouncementForbiddenException(announcementId);
    }

    if (dto.Title is not null)
        announcement.Title = dto.Title;

    if (dto.Description is not null)
        announcement.Description = dto.Description;

    if (dto.Type.HasValue)
        announcement.Type = dto.Type.Value;

    if (dto.IsActive.HasValue)
        announcement.IsActive = dto.IsActive.Value;

    if (!string.IsNullOrWhiteSpace(dto.City))
    {
        var location = await dbContext.Locations
            .FirstOrDefaultAsync(
                l => l.City.ToLower() == dto.City.ToLower(),
                cancellationToken);

        if (location is null)
        {
            location = new Location
            {
                City = dto.City,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Locations.Add(location);
        }

        announcement.Location = location;
    }

    if (dto.InstrumentIds is not null)
    {
        var instruments = await dbContext.Instruments
            .Where(i => dto.InstrumentIds.Contains(i.Id))
            .ToListAsync(cancellationToken);

        announcement.AnnouncementInstruments.Clear();

        foreach (var instrument in instruments)
        {
            announcement.AnnouncementInstruments.Add(new AnnouncementInstrument
            {
                Instrument = instrument
            });
        }
    }

    if (dto.TagIds is not null)
    {
        var tags = await dbContext.Tags
            .Where(t => dto.TagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        announcement.Tags = tags;
    }

    if (dto.StyleIds is not null)
    {
        var styles = await dbContext.Styles
            .Where(s => dto.StyleIds.Contains(s.Id))
            .ToListAsync(cancellationToken);

        announcement.Styles = styles;
    }

    await dbContext.SaveChangesAsync(cancellationToken);

    return announcement.ToDto();
}

    public async Task DeleteAnnouncementAsync(Guid id, Guid musicianId, CancellationToken cancellationToken)
    {
        var foundAnnouncement = await dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (foundAnnouncement == null)
        {
            throw new AnnouncementException.AnnouncementNotFoundException(id);
        }

        if (foundAnnouncement.MusicianId != musicianId)
        {
            throw new AnnouncementException.AnnouncementForbiddenException(id);
        }

        dbContext.Announcements.Remove(foundAnnouncement);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
