using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BandR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController(IConversationService conversationService, IMusicianService musicianService) : ControllerBase
{
    // GET
    [HttpGet("{conversationId}")]
    public async Task<ActionResult<ConversationDto>> GetConversation([FromRoute] Guid conversationId, CancellationToken cancellationToken)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), cancellationToken);
        return Ok(await conversationService.GetConversation(musician.Id, conversationId, cancellationToken));
    }
    
    [HttpPost]
    public async Task<ActionResult<ConversationDto>> CreateConversation([FromBody] CreateConversationDto conversationDto, CancellationToken cancellationToken)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), cancellationToken);
        return Ok(await conversationService.CreateConversation(musician.Id, conversationDto, cancellationToken));
    }
    
    [HttpPost("{conversationId}")]
    public async Task<ActionResult<ConversationDto>> SendMessage([FromRoute] Guid conversationId, [FromBody] CreateMessageDto createMessageDto, CancellationToken cancellationToken)
    {
        var musician = await musicianService.GetMusicianByUserIdAsync(User.GetUserId(), cancellationToken);
        return Ok(await conversationService.SendMessage(musician.Id, conversationId, createMessageDto, cancellationToken));
    }
}