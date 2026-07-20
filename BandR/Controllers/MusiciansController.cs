using BandR.Common;
using BandR.DTOs.Announcements;
using BandR.DTOs.Musicians;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusiciansController(
    IMusicianService musicianService,
    IAnnouncementService announcementService
) : Controller
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<MusicianListDto>>> GetMusicians(CancellationToken ct)
    {
        return Ok(await musicianService.GetMusiciansAsync(ct));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<MusicianDto>> GetMusician([FromRoute] Guid id, CancellationToken ct)
    {
        return Ok(await musicianService.GetMusicianByIdAsync(id, ct));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateMusician(CreateMusicianDto dto, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var musician = await musicianService.CreateMusicianAsync(dto, userId, ct);
        return CreatedAtAction(nameof(GetMusician), new { id = musician.Id }, musician);
    }

    [HttpGet("{id}/announcements")]
    [Authorize]
    public async Task<ActionResult<PagedResponse<AnnouncementListDto>>> GetAnnouncementsForMusician(
        [FromRoute] Guid id,
        [FromQuery] AnnouncementQueryFilter filter,
        CancellationToken ct)
    {
        return Ok(await announcementService.GetAnnouncementsForMusicianAsync(id, filter, ct));
    }
    
    [HttpGet("me/announcements")]
    [Authorize]
    public async Task<ActionResult<PagedResponse<AnnouncementListDto>>> GetMyAnnouncements(
        [FromQuery] AnnouncementQueryFilter filter,
        CancellationToken ct)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), ct);
        return Ok(await announcementService.GetAnnouncementsForMusicianAsync(musician.Id, filter, ct));
    }
}
