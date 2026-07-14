using BandR.Data;
using BandR.DTOs.Conversation;
using BandR.Entities;
using BandR.Entities.Joints;
using BandR.Extensions;
using BandR.Services;

namespace BandR.Tests.IntegrationTests.Services;

public class ConversationServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly ConversationService _conversationService;
    private readonly Guid _musicianId;
    private readonly ApplicationDbContext _dbContext;

    public ConversationServiceTests(TestDatabaseFixture fixture)
    {
        _conversationService = new ConversationService(fixture.DbContext);
        _dbContext = fixture.DbContext;
    }

    private async Task<ConversationDto> CreateConversation(Guid? announcementId, Guid otherMusicianId)
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            AnnouncementId = announcementId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        var musicianConversation = new MusicianConversation
        {
            ConversationId = conversation.Id,
            MusicianId = _musicianId,
            JoinedAt = DateTime.UtcNow,
        };
        var otherMusicianConversation = new MusicianConversation
        {
            ConversationId = conversation.Id,
            MusicianId = otherMusicianId,
            JoinedAt = DateTime.UtcNow,
        };
        conversation.MusicianConversations.Add(musicianConversation);
        conversation.MusicianConversations.Add(otherMusicianConversation);
        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync();
        return conversation.ToDto();
    }
}