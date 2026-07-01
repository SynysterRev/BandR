using BandR.DTOs.Musicians;

namespace BandR.Services.Interfaces;

public interface IMusicianService
{
    public Task<MusicianDto> CreateMusicianAsync(CreateMusicianDto musician, Guid appUserId, CancellationToken ct);
    public Task<MusicianDto> UpdateMusicianAsync(Guid id, UpdateMusicianDto dto, Guid appUserId, CancellationToken ct);
    public Task DeleteMusicianAsync(Guid id, CancellationToken ct);
    public Task<MusicianDto> GetMusicianByIdAsync(Guid id, CancellationToken ct);
    public Task<List<MusicianListDto>> GetMusiciansAsync(CancellationToken ct);
}