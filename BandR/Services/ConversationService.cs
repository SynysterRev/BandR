using BandR.Data;
using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;
using BandR.Entities;
using BandR.Entities.Joints;
using BandR.Exceptions;
using BandR.Extensions;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class ConversationService(ApplicationDbContext dbContext) : IConversationService
{
    public async Task<ConversationDto> CreateConversation(Guid musicianId, CreateConversationDto conversationDto,
        CancellationToken cancellationToken)
    {
        var otherMusician = await dbContext.Musicians
            .Include(m => m.AppUser)
            .FirstOrDefaultAsync(m => m.Id == conversationDto.OtherMusicianId, cancellationToken);
        if (otherMusician is null)
        {
            throw new MusicianException.MusicianNotFoundException(conversationDto.OtherMusicianId);
        }

        if (otherMusician.AppUser.DeactivatedAt is not null)
        {
            throw new ConversationException.ParticipantUnavailableException(otherMusician.Id);
        }

        if (conversationDto.AnnouncementId is not null)
        {
            var announcement =
                await dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == conversationDto.AnnouncementId.Value,
                    cancellationToken);
            if (announcement is null)
            {
                throw new AnnouncementException.AnnouncementNotFoundException(conversationDto.AnnouncementId.Value);
            }

            var foundConv = await dbContext.Conversations
                .Include(c => c.Messages)
                .ThenInclude(message => message.Sender)
                .Where(c => c.AnnouncementId == announcement.Id)
                .Where(c => c.MusicianConversations.Any(mc => mc.MusicianId == musicianId) &&
                            c.MusicianConversations.Any(mc => mc.MusicianId == conversationDto.OtherMusicianId))
                .FirstOrDefaultAsync(cancellationToken);
            if (foundConv is not null)
            {
                return foundConv.ToDto();
            }
        }
        else
        {
            var foundConv = await dbContext.Conversations
                .Include(c => c.Messages)
                .ThenInclude(message => message.Sender)
                .Where(c => c.MusicianConversations.Any(mc => mc.MusicianId == musicianId) &&
                            c.MusicianConversations.Any(mc => mc.MusicianId == conversationDto.OtherMusicianId))
                .FirstOrDefaultAsync(cancellationToken);
            if (foundConv is not null)
            {
                return foundConv.ToDto();
            }
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
        return conversation.ToDto();
    }

    public async Task<MessageDto> SendMessage(Guid musicianId, Guid conversationId, CreateMessageDto message,
        CancellationToken cancellationToken)
    {
        var conversation =
            await dbContext.Conversations
                .Include(c => c.MusicianConversations)
                .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);
        if (conversation is null)
        {
            throw new ConversationException.ConversationNotFoundException(conversationId);
        }

        if (conversation.MusicianConversations.All(mc => mc.MusicianId != musicianId))
        {
            throw new ConversationException.ConversationForbiddenException(conversationId);
        }

        if (!conversation.IsActive)
        {
            throw new ConversationException.ConversationUnavailableException(conversationId);
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
            throw new ConversationException.ConversationNotFoundException(conversationId);
        }

        if (conversation.MusicianConversations.All(mc => mc.MusicianId != musicianId))
        {
            throw new ConversationException.ConversationForbiddenException(conversationId);
        }

        return conversation.ToDto();
    }
}
