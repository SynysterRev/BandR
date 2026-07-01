using BandR.Common;
using BandR.DTOs.Announcements;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController(IAnnouncementService announcementService) : ControllerBase
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
        var userId = User.GetUserId();
        var announcement = await announcementService.CreateAnnouncementAsync(dto, userId, ct);
        return CreatedAtAction(nameof(GetAnnouncementById), new { id = announcement.Id }, announcement);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DisableAnnouncementById([FromRoute] Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        await announcementService.DeleteAnnouncementAsync(id, userId, ct);
        return NoContent();
    }
}