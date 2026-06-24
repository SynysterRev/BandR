using BandR.DTOs.Musicians;

namespace BandR.Services.Interfaces;

public interface IMusicianService
{
    public Task<MusicianDto> CreateMusician(CreateMusicianDto musician, Guid appUserId, CancellationToken ct);
    public Task<MusicianDto> UpdateMusician(Guid id, UpdateMusicianDto dto, Guid appUserId, CancellationToken ct);
    public Task DeleteMusician(Guid id, CancellationToken ct);
    public Task<MusicianDto> GetMusicianById(Guid id, CancellationToken ct);
    public Task<List<MusicianListDto>> GetMusicians(CancellationToken ct);
}