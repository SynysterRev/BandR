using System.Security.Claims;
using BandR.Controllers;
using BandR.DTOs.Announcements;
using BandR.DTOs.Musicians;
using BandR.Entities;
using BandR.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BandR.Tests.Unit.Controllers;

public class AnnouncementsControllerTests
{
    [Fact]
    public async Task UpdateAnnouncement_ShouldUpdateUsingTheCurrentMusician()
    {
        var userId = Guid.NewGuid();
        var musicianId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        var dto = new UpdateAnnouncementDto("Updated title", null, null, null, null, null, null, null);
        var updatedAnnouncement = new AnnouncementDto(
            announcementId,
            "Updated title",
            "Description",
            "Montpellier",
            musicianId,
            "musician",
            AnnouncementType.LookingForBand,
            [],
            [],
            [],
            true,
            DateTime.UtcNow);
        var musicianService = new Mock<IMusicianService>();
        musicianService
            .Setup(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MusicianDto(musicianId, "musician", "Montpellier", [], [], [], null, null));
        var announcementService = new Mock<IAnnouncementService>();
        announcementService
            .Setup(service => service.UpdateAnnouncementAsync(
                announcementId,
                musicianId,
                dto,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedAnnouncement);
        var controller = new AnnouncementsController(announcementService.Object, musicianService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        [new Claim(ClaimTypes.NameIdentifier, userId.ToString())]))
                }
            }
        };

        var result = await controller.UpdateAnnouncement(announcementId, dto, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        announcementService.Verify(service => service.UpdateAnnouncementAsync(
            announcementId,
            musicianId,
            dto,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
