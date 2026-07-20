using BandR.Data;
using BandR.DTOs.Conversation;
using BandR.Entities;
using BandR.Services;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using Respawn;

namespace BandR.Tests.IntegrationTests.Services;

public class AccountServiceTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private readonly ApplicationDbContext _dbContext;

    public AccountServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dbContext = fixture.DbContext;
    }

    public async Task InitializeAsync()
    {
        using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();
        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = [new Respawn.Graph.Table("__EFMigrationsHistory")]
        });
        await respawner.ResetAsync(connection);

        _dbContext.ChangeTracker.Clear();
        await _fixture.SeedDefaultDataAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task DeactivateAccountAsync_ShouldRollbackAllChanges_WhenTokenRevocationFails()
    {
        var musician = await _fixture.CreateDefaultMusician();
        var announcement = await _fixture.CreateDefaultAnnouncement(musicianId: musician.Id);
        var otherMusician = await _fixture.CreateDefaultMusician(customUserId: Guid.NewGuid());
        var conversation = await new ConversationService(_dbContext).CreateConversation(
            musician.Id,
            new CreateConversationDto(null, otherMusician.Id),
            CancellationToken.None);
        var tokenService = new Mock<ITokenService>();
        tokenService
            .Setup(service => service.RemoveTokensForUserAsync(_fixture.AppUserId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Token storage is unavailable."));
        var service = new AccountService(CreateUserManager(), tokenService.Object, _dbContext);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.DeactivateAccountAsync(_fixture.AppUserId, CancellationToken.None));

        _dbContext.ChangeTracker.Clear();
        var user = await _dbContext.Users.FindAsync(_fixture.AppUserId);
        var savedAnnouncement = await _dbContext.Announcements.FindAsync(announcement.Id);
        var savedConversation = await _dbContext.Conversations.FindAsync(conversation.Id);

        Assert.Null(user!.DeactivatedAt);
        Assert.True(savedAnnouncement!.IsActive);
        Assert.True(savedConversation!.IsActive);
    }

    private UserManager<ApplicationUser> CreateUserManager()
    {
        var store = new UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>(_dbContext);
        return new UserManager<ApplicationUser>(
            store,
            Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(),
            [],
            [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new Mock<IServiceProvider>().Object,
            NullLogger<UserManager<ApplicationUser>>.Instance);
    }
}
