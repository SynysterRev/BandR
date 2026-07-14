using BandR.Common;
using BandR.Data;
using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Services;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;

namespace BandR.Tests.IntegrationTests.Services;

public sealed class AnnouncementServiceTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private readonly IAnnouncementService _announcementService;
    private readonly ApplicationDbContext _dbContext;

    public AnnouncementServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dbContext = fixture.DbContext;
        _announcementService = new AnnouncementService(_dbContext);
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
    // ---- Helpers ----

    // ---- CreateAnnouncement ----

    [Fact]
    public async Task CreateAnnouncement_ShouldReturnAnnouncementDto()
    {
        var result = await _fixture.CreateDefaultAnnouncement();

        Assert.NotNull(result);
        Assert.Equal("Looking for Bassist", result.Title);
        Assert.Equal("London", result.City);
    }

    [Fact]
    public async Task CreateAnnouncement_ShouldReuseExistingLocation()
    {
        await _fixture.CreateDefaultAnnouncement(title: "Announcement 1", city: "Manchester");
        await _fixture.CreateDefaultAnnouncement(title: "Announcement 2", city: "Manchester");

        var locationCount = await _dbContext.Locations
            .CountAsync(l => l.City.ToLower() == "manchester");

        Assert.Equal(1, locationCount);
    }

    // ---- GetAnnouncementById ----

    [Fact]
    public async Task GetAnnouncementById_ShouldReturnAnnouncement()
    {
        var created = await _fixture.CreateDefaultAnnouncement();

        var result = await _announcementService.GetAnnouncementByIdAsync(created.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Looking for Bassist", result.Title);
    }

    [Fact]
    public async Task GetAnnouncementById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.GetAnnouncementByIdAsync(Guid.NewGuid(), CancellationToken.None)
        );
    }

    // ---- GetAnnouncements (Filtered/Active) ----

    [Fact]
    public async Task GetAnnouncements_ShouldReturnOnlyActiveAnnouncements()
    {
        await _fixture.CreateDefaultAnnouncement(title: "Active 1");

        var inactive = await _fixture.CreateDefaultAnnouncement(title: "Inactive 1");
        var entity = await _dbContext.Announcements.FindAsync(inactive.Id);
        entity!.IsActive = false;
        await _dbContext.SaveChangesAsync();

        var filter = new AnnouncementQueryFilter { PageNumber = 1, PageSize = 10 };
        var result = await _announcementService.GetAnnouncementsAsync(filter, CancellationToken.None);

        Assert.Contains(result.Data, a => a.Title == "Active 1");
        Assert.DoesNotContain(result.Data, a => a.Title == "Inactive 1");
    }

    // ---- GetAnnouncementsForMusician ----

    [Fact]
    public async Task GetAnnouncementsForMusician_ShouldReturnOnlyMusicianAnnouncements()
    {
        var existingLocation = await _dbContext.Locations.FirstAsync();

        var otherMusicianId = Guid.NewGuid();
        var otherUser = new ApplicationUser
        {
            Id = otherMusicianId,
            UserName = "otheruser",
            Email = "other@test.com",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        await _dbContext.Users.AddAsync(otherUser);
        await _dbContext.SaveChangesAsync();

        var musician = await _dbContext.Musicians.AddAsync(new Musician
        {
            Id = otherMusicianId,
            AppUserId = otherMusicianId,
            LocationId = existingLocation.Id,
            Username = "Other",
            CreatedAt = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();

        await _fixture.CreateDefaultAnnouncement(title: "My great announcement", musicianId: musician.Entity.Id);
        await _fixture.CreateDefaultAnnouncement(title: "Someone else's announcement", musicianId: otherMusicianId);

        var filter = new AnnouncementQueryFilter { PageNumber = 1, PageSize = 1 };
        var result =
            await _announcementService.GetAnnouncementsForMusicianAsync(musician.Entity.Id, filter,
                CancellationToken.None);

        Assert.Single(result.Data);
        Assert.Equal("My great announcement", result.Data.First().Title);
    }

    // ---- UpdateAnnouncement ----

    [Fact]
    public async Task UpdateAnnouncement_ShouldUpdateFields()
    {
        var musician = await _fixture.CreateDefaultMusician();

        var created = await _fixture.CreateDefaultAnnouncement(musicianId: musician.Id);

        var updateDto = new UpdateAnnouncementDto(
            Title: "Updated Title",
            Description: "New description",
            Type: null,
            IsActive: false,
            City: "Liverpool",
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result =
            await _announcementService.UpdateAnnouncementAsync(created.Id, musician.Id, updateDto,
                CancellationToken.None);

        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("New description", result.Description);
        Assert.Equal("Liverpool", result.City);
        Assert.False(result.IsActive);
    }

    [Fact]
    public async Task UpdateAnnouncement_ShouldThrow_WhenForbidden()
    {
        var created = await _fixture.CreateDefaultAnnouncement();
        var fakeMusicianId = Guid.NewGuid();

        var updateDto = new UpdateAnnouncementDto(
            Title: "Hacked", Description: null, Type: null, IsActive: null, City: null, InstrumentIds: null,
            StyleIds: null, TagIds: null
        );

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementForbiddenException>(() =>
            _announcementService.UpdateAnnouncementAsync(created.Id, fakeMusicianId, updateDto, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateAnnouncement_ShouldThrow_WhenNotFound()
    {
        var musician = await _fixture.CreateDefaultMusician();
        var updateDto = new UpdateAnnouncementDto(
            Title: "Oops", Description: null, Type: null, IsActive: null, City: null, InstrumentIds: null,
            StyleIds: null, TagIds: null
        );

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.UpdateAnnouncementAsync(Guid.NewGuid(), musician.Id, updateDto, CancellationToken.None)
        );
    }

    // ---- DeleteAnnouncement ----

    [Fact]
    public async Task DeleteAnnouncement_ShouldRemoveFromDatabase()
    {
        var musician = await _fixture.CreateDefaultMusician();
        var created = await _fixture.CreateDefaultAnnouncement(musicianId: musician.Id);

        await _announcementService.DeleteAnnouncementAsync(created.Id, musician.Id, CancellationToken.None);

        var exists = await _dbContext.Announcements.AnyAsync(a => a.Id == created.Id);
        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenForbidden()
    {
        var created = await _fixture.CreateDefaultAnnouncement();
        var fakeMusicianId = Guid.NewGuid();

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementForbiddenException>(() =>
            _announcementService.DeleteAnnouncementAsync(created.Id, fakeMusicianId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenNotFound()
    {
        var musician = await _fixture.CreateDefaultMusician();
        
        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.DeleteAnnouncementAsync(Guid.NewGuid(), musician.Id, CancellationToken.None)
        );
    }
}