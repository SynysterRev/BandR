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
    public async Task GetMyMusician_ShouldUseTheCurrentUser()
    {
        var userId = Guid.NewGuid();
        var musicianId = Guid.NewGuid();
        var musicianService = new Mock<IMusicianService>();
        musicianService
            .Setup(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MusicianDto(musicianId, "musician", "Montpellier", [], [], [], null, null));
        var controller = CreateController(musicianService.Object, new Mock<IAnnouncementService>().Object, userId);

        var result = await controller.GetMyMusician(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        musicianService.Verify(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMusician_ShouldUpdateUsingTheCurrentUser()
    {
        var userId = Guid.NewGuid();
        var musicianId = Guid.NewGuid();
        var dto = new UpdateMusicianDto("updated", null, null, null, null, null);
        var updatedMusician = new MusicianDto(musicianId, "updated", "Montpellier", [], [], [], null, null);
        var musicianService = new Mock<IMusicianService>();
        musicianService
            .Setup(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MusicianDto(musicianId, "musician", "Montpellier", [], [], [], null, null));
        musicianService
            .Setup(service => service.UpdateMusicianAsync(
                musicianId,
                dto,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedMusician);
        var controller = CreateController(musicianService.Object, new Mock<IAnnouncementService>().Object, userId);

        var result = await controller.UpdateMusician(dto, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        musicianService.Verify(service => service.UpdateMusicianAsync(
            musicianId,
            dto,
            userId,
            It.IsAny<CancellationToken>()), Times.Once);
    }

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
        var controller = CreateController(musicianService.Object, announcementService.Object, userId);

        var result = await controller.GetMyAnnouncements(filter, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        announcementService.Verify(service => service.GetAnnouncementsForMusicianAsync(
            musicianId,
            filter,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static MusiciansController CreateController(
        IMusicianService musicianService,
        IAnnouncementService announcementService,
        Guid userId)
    {
        return new MusiciansController(musicianService, announcementService)
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
    }
}
