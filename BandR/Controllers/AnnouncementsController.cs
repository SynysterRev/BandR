using BandR.Common;
using BandR.DTOs.Announcements;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController(IAnnouncementService announcementService, IMusicianService musicianService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResponse<AnnouncementListDto>>> GetAnnouncements(
        [FromQuery] AnnouncementQueryFilter filter, CancellationToken ct)
    {
        return Ok(await announcementService.GetAnnouncementsAsync(filter, ct));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncementById([FromRoute] Guid id, CancellationToken ct)
    {
        return Ok(await announcementService.GetAnnouncementByIdAsync(id, ct));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto dto, CancellationToken ct)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), ct);
        var announcement = await announcementService.CreateAnnouncementAsync(dto, musician.Id, ct);
        return CreatedAtAction(nameof(GetAnnouncementById), new { id = announcement.Id }, announcement);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(
        [FromRoute] Guid id,
        [FromBody] UpdateAnnouncementDto dto,
        CancellationToken ct)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), ct);
        return Ok(await announcementService.UpdateAnnouncementAsync(id, musician.Id, dto, ct));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DisableAnnouncementById([FromRoute] Guid id, CancellationToken ct)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), ct);
        await announcementService.DeleteAnnouncementAsync(id, musician.Id, ct);
        return NoContent();
    }
}
