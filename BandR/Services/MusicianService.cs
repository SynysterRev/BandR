using BandR.Data;
using BandR.DTOs.Musicians;
using BandR.Entities;
using BandR.Entities.Joints;
using BandR.Exceptions;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class MusicianService(ApplicationDbContext dbContext) : IMusicianService
{
    public async Task<MusicianDto> CreateMusicianAsync(CreateMusicianDto musicianDto, Guid appUserId, CancellationToken ct)
    {
        var location = await dbContext.Locations
            .FirstOrDefaultAsync(l => l.City.ToLower() == musicianDto.City.ToLower(), ct);

        if (location is null)
        {
            location = new Location
            {
                City = musicianDto.City,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Locations.Add(location);
        }

        var musician = musicianDto.ToEntity(appUserId, location);
        if (musicianDto.InstrumentIds is not null && musicianDto.InstrumentIds.Count > 0)
        {
            var instruments = await dbContext.Instruments
                .Where(i => musicianDto.InstrumentIds.Contains(i.Id))
                .ToListAsync(ct);

            musician.MusicianInstruments = instruments.Select(i => new MusicianInstrument
            {
                Instrument = i
            }).ToList();
        }

        if (musicianDto.TagIds is not null && musicianDto.TagIds.Count > 0)
        {
            var tags = await dbContext.Tags
                .Where(t => musicianDto.TagIds.Contains(t.Id))
                .ToListAsync(ct);

            musician.Tags = tags;
        }

        if (musicianDto.StyleIds is not null && musicianDto.StyleIds.Count > 0)
        {
            var styles = await dbContext.Styles
                .Where(s => musicianDto.StyleIds.Contains(s.Id))
                .ToListAsync(ct);

            musician.Styles = styles;
        }

        dbContext.Musicians.Add(musician);
        await dbContext.SaveChangesAsync(ct);
        return musician.ToDto();
    }

    public async Task<MusicianDto> UpdateMusicianAsync(Guid id, UpdateMusicianDto dto, Guid appUserId, CancellationToken ct)
    {
        var musician = await dbContext.Musicians
                           .Include(m => m.Location)
                           .Include(m => m.MusicianInstruments)
                           .ThenInclude(mi => mi.Instrument)
                           .Include(m => m.Styles)
                           .Include(m => m.Tags)
                           .FirstOrDefaultAsync(m => m.Id == id, ct)
                       ?? throw new MusicianException.MusicianNotFoundException(id);

        if (musician.AppUserId != appUserId)
            throw new MusicianException.MusicianForbiddenException(id);

        if (dto.Username is not null)
            musician.Username = dto.Username;

        if (dto.Bio is not null)
            musician.Bio = dto.Bio;

        if (dto.City is not null)
        {
            var location = await dbContext.Locations
                .FirstOrDefaultAsync(l => l.City.ToLower() == dto.City.ToLower(), ct);

            if (location is null)
            {
                location = new Location
                {
                    City = dto.City,
                    CreatedAt = DateTime.UtcNow
                };
                dbContext.Locations.Add(location);
            }

            musician.Location = location;
        }

        if (dto.InstrumentIds is not null)
        {
            dbContext.RemoveRange(musician.MusicianInstruments);
            var instruments = await dbContext.Instruments
                .Where(i => dto.InstrumentIds.Contains(i.Id))
                .ToListAsync(ct);
            musician.MusicianInstruments = instruments
                .Select(i => new MusicianInstrument { Instrument = i }).ToList();
        }

        if (dto.StyleIds is not null)
        {
            musician.Styles = await dbContext.Styles
                .Where(s => dto.StyleIds.Contains(s.Id))
                .ToListAsync(ct);
        }

        if (dto.TagIds is not null)
        {
            musician.Tags = await dbContext.Tags
                .Where(t => dto.TagIds.Contains(t.Id))
                .ToListAsync(ct);
        }

        musician.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(ct);

        return musician.ToDto();
    }

    public async Task<MusicianDto> GetMusicianByIdAsync(Guid id, CancellationToken ct)
    {
        var foundMusician = await dbContext.Musicians
            .Include(m => m.Location)
            .Include(m => m.MusicianInstruments).ThenInclude(mi => mi.Instrument)
            .Include(m => m.Styles)
            .Include(m => m.Tags)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.Id == id, ct);
        if (foundMusician == null)
        {
            throw new MusicianException.MusicianNotFoundException(id);
        }

        return foundMusician.ToDto();
    }

    public async Task<MusicianDto> GetMusicianByUserIdAsync(Guid appUserId, CancellationToken ct)
    {
        var foundMusician = await dbContext.Musicians
            .Include(m => m.Location)
            .Include(m => m.MusicianInstruments).ThenInclude(mi => mi.Instrument)
            .Include(m => m.Styles)
            .Include(m => m.Tags)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.AppUserId == appUserId, ct);
        if (foundMusician == null)
        {
            throw new MusicianException.MusicianProfileNotFoundForUserException(appUserId);
        }

        return foundMusician.ToDto();
    }

    public async Task<List<MusicianListDto>> GetMusiciansAsync(CancellationToken ct)
    {
        return await dbContext.Musicians.Select(m => new MusicianListDto(m.Id, m.Username, m.Location.City))
            .ToListAsync(cancellationToken: ct);
    }
}
