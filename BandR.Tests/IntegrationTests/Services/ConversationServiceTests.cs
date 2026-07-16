using BandR.Data;
using BandR.DTOs.Conversation;
using BandR.DTOs.Messages;
using BandR.DTOs.Musicians;
using BandR.Exceptions;
using BandR.Services;
using FluentAssertions;
using Npgsql;
using Respawn;

namespace BandR.Tests.IntegrationTests.Services;

public class ConversationServiceTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private readonly ConversationService _conversationService;
    private readonly ApplicationDbContext _dbContext;

    public ConversationServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _conversationService = new ConversationService(fixture.DbContext);
        _dbContext = fixture.DbContext;
    }

    public async Task InitializeAsync()
    {
        using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        });

        await respawner.ResetAsync(connection);

        _dbContext.ChangeTracker.Clear();

        await _fixture.SeedDefaultDataAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<(ConversationDto Conversation, MusicianDto Musician)> CreateConversation(Guid? appUserId, Guid? announcementId)
    {
        var musician = await _fixture.CreateDefaultMusician(customUserId: appUserId ?? Guid.NewGuid());
        var otherMusician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var createConversation = new CreateConversationDto
        (
            announcementId,
            otherMusician.Id
        );
        return (await _conversationService.CreateConversation(musician.Id, createConversation, CancellationToken.None), musician);
    }

    [Fact]
    private async Task ShouldCreateWithoutAnnouncement()
    {
        var (conversationDto, _) = await CreateConversation(null, null);
        
        conversationDto.Should().NotBeNull();
        conversationDto.Messages.Count.Should().Be(0);
    }
    
    [Fact]
    private async Task ShouldCreatedWithAnnouncement()
    {
        var announcement = await _fixture.CreateDefaultAnnouncement();
        var (conversationDto, _) = await CreateConversation(null, announcement.Id);
        
        conversationDto.Should().NotBeNull();
        conversationDto.Messages.Count.Should().Be(0);
        conversationDto.AnnouncementId.Should().Be(announcement.Id);
    }
    
    [Fact]
    private async Task ShouldFailWhenAnnouncementDoesNotExist()
    {
        var announcementId = Guid.NewGuid();
        var act = () => CreateConversation(null, announcementId);
        
        await act.Should().ThrowAsync<AnnouncementException.AnnouncementNotFoundException>().WithMessage($"Announcement with id {announcementId} not found");
    }

    [Fact]
    private async Task ShouldReturnAlreadyExistingConversation()
    {
        var announcement = await _fixture.CreateDefaultAnnouncement();
        var musician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var otherMusician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var createConversation = new CreateConversationDto
        (
            announcement.Id,
            otherMusician.Id
        );
        
        var conversationDto = await _conversationService.CreateConversation(musician.Id, createConversation, CancellationToken.None);
        var sameConv = await _conversationService.CreateConversation(musician.Id, createConversation, CancellationToken.None);
        
        sameConv.Should().NotBeNull();
        sameConv.Id.Should().Be(conversationDto.Id);
    }
    
    [Fact]
    private async Task ShouldSendAMessage()
    {
        var appUserId = Guid.NewGuid();
        var (conversationDto, musician) = await CreateConversation(appUserId, null);
        var message = new CreateMessageDto("This is a test message");
        var messageDto = await _conversationService.SendMessage(musician.Id, conversationDto.Id, message, CancellationToken.None);
        
        messageDto.Should().NotBeNull();
        messageDto.Content.Should().Be("This is a test message");
        messageDto.SenderName.Should().Be(musician.Username);
    }
    
    [Fact]
    private async Task ShouldFailWhenConversationDoesNotExist()
    {
        var musician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var message = new CreateMessageDto("This is a test message");
        var convId = Guid.NewGuid();
        
        var act = () => _conversationService.SendMessage(musician.Id, convId, message, CancellationToken.None);
        
        await act.Should().ThrowAsync<ConversationException.ConversationNotFoundException>().WithMessage($"Conversation with id {convId} not found");
    }
    
    [Fact]
    private async Task ShouldFailWhenMusicianIsNotInConversation()
    {
        var appUserId = Guid.NewGuid();
        var (conversationDto, _) = await CreateConversation(appUserId, null);
        var message = new CreateMessageDto("This is a test message");
        var convId = Guid.NewGuid();
        
        var act = () => _conversationService.SendMessage(Guid.NewGuid(), conversationDto.Id, message, CancellationToken.None);
        
        await act.Should().ThrowAsync<ConversationException.ConversationForbiddenException>().WithMessage($"Access to Conversation {conversationDto.Id} is forbidden");
    }
}