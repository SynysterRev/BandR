using BandR.DTOs;
using BandR.DTOs.Musicians;
using BandR.Entities;

namespace BandR.Extensions;

public static class MusicianExtensions
{
    public static Musician ToEntity(this CreateMusicianDto dto, Guid appUserId, Location location)
    {
        return new Musician
        {
            Username = dto.Username,
            Bio = dto.Bio,
            AppUserId = appUserId,
            Location = location
        };
    }
    
    public static MusicianDto ToDto(this Musician musician)
    {
        return new MusicianDto(
            musician.Id,
            musician.Username,
            musician.Location.City,
            musician.MusicianInstruments.Select(mi => mi.Instrument.Name).ToList(),
            musician.Tags.Select(t => t.Name).ToList(),
            musician.Styles.Select(s => s.Name).ToList(),
            musician.Bio,
            musician.AvatarUrl
        );
    }
    
    public static Musician ToEntity(this MusicianDto musicianDto, Guid appUserId, Location location)
    {
        return new Musician
        {
            Username = musicianDto.Username,
            Bio = musicianDto.Bio,
            AppUserId = appUserId,
            Location = location
        };
    }
}