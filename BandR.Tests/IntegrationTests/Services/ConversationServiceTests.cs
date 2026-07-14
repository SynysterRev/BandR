using BandR.Data;
using BandR.DTOs.Conversation;
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

    private async Task<ConversationDto> CreateConversation(Guid? announcementId)
    {
        var musician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var otherMusician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var createConversation = new CreateConversationDto
        (
            announcementId,
            otherMusician.Id
        );
        return await _conversationService.CreateConversation(musician.Id, createConversation, CancellationToken.None);
    }

    [Fact]
    private async Task ShouldCreateWithoutAnnouncement()
    {
        var conversationDto = await CreateConversation(null);
        
        conversationDto.Should().NotBeNull();
        conversationDto.Messages.Count.Should().Be(0);
    }
    
    [Fact]
    private async Task ShouldCreatedWithAnnouncement()
    {
        var announcement = await _fixture.CreateDefaultAnnouncement();
        var conversationDto = await CreateConversation(announcement.Id);
        
        conversationDto.Should().NotBeNull();
        conversationDto.Messages.Count.Should().Be(0);
        conversationDto.AnnouncementId.Should().Be(announcement.Id);
    }
    
    [Fact]
    private async Task ShouldFailWhenAnnouncementDoesNotExist()
    {
        var announcementId = Guid.NewGuid();
        var act = () => CreateConversation(announcementId);
        
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
}