using System.Security.Claims;
using BandR.Controllers;
using BandR.DTOs.Conversation;
using BandR.DTOs.Musicians;
using BandR.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BandR.Tests.Unit.Controllers;

public class ConversationsControllerTests
{
    [Fact]
    public async Task GetConversations_ShouldUseTheCurrentMusicianId()
    {
        var userId = Guid.NewGuid();
        var musicianId = Guid.NewGuid();
        var musicianService = new Mock<IMusicianService>();
        musicianService
            .Setup(service => service.GetMusicianByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MusicianDto(musicianId, "musician", "Paris", [], [], [], null, null));
        var conversationService = new Mock<IConversationService>();
        conversationService
            .Setup(service => service.GetConversationsAsync(musicianId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var controller = new ConversationsController(conversationService.Object, musicianService.Object)
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

        var result = await controller.GetConversations(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        conversationService.Verify(
            service => service.GetConversationsAsync(musicianId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
