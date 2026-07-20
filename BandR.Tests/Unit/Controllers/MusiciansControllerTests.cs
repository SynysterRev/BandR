using System.Security.Claims;
using BandR.Common;
using BandR.Controllers;
using BandR.DTOs.Musicians;
using BandR.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BandR.Tests.Unit.Controllers;

public class MusiciansControllerTests
{
    [Fact]
    public async Task GetMyAnnouncements_ShouldUseTheCurrentMusicianId()
    {
        var userId = Guid.NewGuid();
        var musicianId = Guid.NewGuid();
        var filter = new AnnouncementQueryFilter();
        var musicianService = new Mock<IMusicianService>();
        musicianService
            .Setup(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MusicianDto(musicianId, "musician", "Montpellier", [], [], [], null, null));
        var announcementService = new Mock<IAnnouncementService>();
        announcementService
            .Setup(service => service.GetAnnouncementsForMusicianAsync(
                musicianId,
                filter,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResponse<BandR.DTOs.Announcements.AnnouncementListDto>());
        var controller = new MusiciansController(musicianService.Object, announcementService.Object)
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

        var result = await controller.GetMyAnnouncements(filter, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        announcementService.Verify(service => service.GetAnnouncementsForMusicianAsync(
            musicianId,
            filter,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
