using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;
using BandR.Entities;

namespace BandR.Extensions;

public static class ConversationExtensions
{
    public static MessageDto ToDto(this Message message) =>
        new MessageDto(
            message.Content,
            message.Sender.Username,
            message.ReadAt
        );

    public static ConversationDto ToDto(this Conversation conversation) =>
        new ConversationDto(
            conversation.Id,
            conversation.Messages.Select(m => m.ToDto()).ToList()
        );
}