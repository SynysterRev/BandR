using BandR.DTOs.Musicians;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusiciansController(IMusicianService musicianService) : Controller
{
#if DEBUG
    [HttpGet]
    public async Task<ActionResult<List<MusicianListDto>>> GetMusicians(CancellationToken ct)
    {
        return Ok(await musicianService.GetMusicians(ct));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MusicianDto>> GetMusician([FromRoute] Guid id, CancellationToken ct)
    {
        return Ok(await musicianService.GetMusicianById(id, ct));
    }

    [HttpPost]
    public async Task<ActionResult> CreateMusician(CreateMusicianDto dto, CancellationToken ct)
    {
        var fakeUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var musician = await musicianService.CreateMusician(dto, fakeUserId, ct);
        return CreatedAtAction(nameof(GetMusician), new { id = musician.Id }, musician);
    }
#endif
}