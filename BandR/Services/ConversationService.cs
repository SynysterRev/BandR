using BandR.Data;
using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;
using BandR.Entities;
using BandR.Entities.Joints;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class ConversationService(ApplicationDbContext dbContext) : IConversationService
{
    public async Task<ConversationDto> CreateConversation(Guid musicianId, CreateConversationDto conversationDto,
        CancellationToken cancellationToken)
    {
        if (conversationDto.AnnouncementId is not null)
        {
            var announcement =
                await dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == conversationDto.AnnouncementId.Value,
                    cancellationToken);
            if (announcement is null)
            {
                // throw exception
            }
        }

        var otherMusician =
            await dbContext.Musicians.FirstOrDefaultAsync(m => m.Id == conversationDto.OtherMusicianId,
                cancellationToken);
        if (otherMusician is null)
        {
            // throw exception
        }

        Conversation conversation = new Conversation
        {
            AnnouncementId = conversationDto.AnnouncementId,
            Id = Guid.NewGuid(),
        };
        var musicianConv = new MusicianConversation
        {
            Conversation = conversation,
            ConversationId = conversation.Id,
            JoinedAt = DateTime.UtcNow,
            MusicianId = musicianId,
        };
        var otherMusicianConv = new MusicianConversation
        {
            Conversation = conversation,
            ConversationId = conversation.Id,
            JoinedAt = DateTime.UtcNow,
            MusicianId = conversationDto.OtherMusicianId,
        };
        conversation.MusicianConversations.Add(musicianConv);
        conversation.MusicianConversations.Add(otherMusicianConv);
        dbContext.Conversations.Add(conversation);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ConversationDto(conversation.Id, conversation.Messages.Select(m => m.ToDto()).ToList());
    }

    public async Task<MessageDto> SendMessage(Guid musicianId, Guid conversationId, CreateMessageDto message,
        CancellationToken cancellationToken)
    {
        var conversation =
            await dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);
        if (conversation is null)
        {
            // throw
        }

        var createdMessage = new Message
        {
            Content = message.Content,
            ConversationId = conversation.Id,
            SenderId = musicianId,
            SentAt = DateTime.UtcNow,
        };
        conversation.Messages.Add(createdMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
        return createdMessage.ToDto();
    }

    public async Task<ConversationDto> GetConversation(Guid musicianId, Guid conversationId,
        CancellationToken cancellationToken)
    {
        var conversation = await dbContext.Conversations
            .Include(c => c.MusicianConversations)
            .Include(c => c.Messages)
            .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);
        if (conversation is null)
        {
            // throw
        }

        if (!conversation.MusicianConversations.Any(mc => mc.MusicianId == musicianId))
        {
            // throw
        }

        return conversation.ToDto();
    }
}