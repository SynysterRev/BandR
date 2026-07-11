using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;

namespace BandR.Services.Interfaces;

public interface IConversationService
{
    public Task<ConversationDto> CreateConversation(Guid musicianId, CreateConversationDto conversationDto, CancellationToken cancellationToken);
    public Task<MessageDto> SendMessage(Guid musicianId, Guid conversationId, CreateMessageDto message, CancellationToken cancellationToken);
    public Task<ConversationDto> GetConversation(Guid musicianId, Guid conversationId, CancellationToken cancellationToken);
}