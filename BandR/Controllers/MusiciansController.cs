using BandR.DTOs.Musicians;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusiciansController(IMusicianService musicianService) : Controller
{
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<MusicianListDto>>> GetMusicians(CancellationToken ct)
    {
        return Ok(await musicianService.GetMusicians(ct));
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<MusicianDto>> GetMusician([FromRoute] Guid id, CancellationToken ct)
    {
        return Ok(await musicianService.GetMusicianById(id, ct));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateMusician(CreateMusicianDto dto, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var musician = await musicianService.CreateMusician(dto, userId, ct);
        return CreatedAtAction(nameof(GetMusician), new { id = musician.Id }, musician);
    }
}