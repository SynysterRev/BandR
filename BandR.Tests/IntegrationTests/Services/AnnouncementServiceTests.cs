using BandR.Common;
using BandR.Data;
using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Services;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandR.Tests.IntegrationTests.Services;

public sealed class AnnouncementServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly IAnnouncementService _announcementService;
    private readonly Guid _musicianId;
    private readonly ApplicationDbContext _dbContext;

    public AnnouncementServiceTests(TestDatabaseFixture fixture)
    {
        _dbContext = fixture.DbContext;
        _announcementService = new AnnouncementService(_dbContext);
        _musicianId = fixture.MusicianId;
    }
    // ---- Helpers ----

    private async Task<AnnouncementDto> CreateDefaultAnnouncement(
        string title = "Looking for Bassist",
        string city = "London",
        Guid? musicianId = null)
    {
        var dto = new CreateAnnouncementDto(
            Title: title,
            Description: "For an alternative rock band",
            City: city,
            Type: AnnouncementType.LookingForMusician,
            InstrumentIds: [],
            StyleIds: [],
            TagIds: []
        );

        return await _announcementService.CreateAnnouncementAsync(dto, musicianId ?? _musicianId, CancellationToken.None);
    }

    // ---- CreateAnnouncement ----

    [Fact]
    public async Task CreateAnnouncement_ShouldReturnAnnouncementDto()
    {
        var result = await CreateDefaultAnnouncement();

        Assert.NotNull(result);
        Assert.Equal("Looking for Bassist", result.Title);
        Assert.Equal("London", result.City);
    }

    [Fact]
    public async Task CreateAnnouncement_ShouldReuseExistingLocation()
    {
        await CreateDefaultAnnouncement(title: "Announcement 1", city: "Manchester");
        await CreateDefaultAnnouncement(title: "Announcement 2", city: "Manchester");

        var locationCount = await _dbContext.Locations
            .CountAsync(l => l.City.ToLower() == "manchester");

        Assert.Equal(1, locationCount);
    }

    // ---- GetAnnouncementById ----

    [Fact]
    public async Task GetAnnouncementById_ShouldReturnAnnouncement()
    {
        var created = await CreateDefaultAnnouncement();

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
        await CreateDefaultAnnouncement(title: "Active 1");
        
        var inactive = await CreateDefaultAnnouncement(title: "Inactive 1");
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

        await _dbContext.Musicians.AddAsync(new Musician 
        { 
            Id = otherMusicianId, 
            AppUserId = otherMusicianId,
            LocationId = existingLocation.Id,
            Username = "Other", 
            CreatedAt = DateTime.UtcNow 
        });
        await _dbContext.SaveChangesAsync();

        await CreateDefaultAnnouncement(title: "My great announcement", musicianId: _musicianId);
        await CreateDefaultAnnouncement(title: "Someone else's announcement", musicianId: otherMusicianId);

        var filter = new AnnouncementQueryFilter { PageNumber = 1, PageSize = 10 };
        var result = await _announcementService.GetAnnouncementsForMusicianAsync(_musicianId, filter, CancellationToken.None);

        Assert.Single(result.Data);
        Assert.Equal("My great announcement", result.Data.First().Title);
    }

    // ---- UpdateAnnouncement ----

    [Fact]
    public async Task UpdateAnnouncement_ShouldUpdateFields()
    {
        var created = await CreateDefaultAnnouncement();

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

        var result = await _announcementService.UpdateAnnouncementAsync(created.Id, _musicianId, updateDto, CancellationToken.None);

        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("New description", result.Description);
        Assert.Equal("Liverpool", result.City);
        Assert.False(result.IsActive);
    }

    [Fact]
    public async Task UpdateAnnouncement_ShouldThrow_WhenForbidden()
    {
        var created = await CreateDefaultAnnouncement();
        var fakeMusicianId = Guid.NewGuid();

        var updateDto = new UpdateAnnouncementDto(
            Title: "Hacked", Description: null, Type: null, IsActive: null, City: null, InstrumentIds: null, StyleIds: null, TagIds: null
        );

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementForbiddenException>(() =>
            _announcementService.UpdateAnnouncementAsync(created.Id, fakeMusicianId, updateDto, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateAnnouncement_ShouldThrow_WhenNotFound()
    {
        var updateDto = new UpdateAnnouncementDto(
            Title: "Oops", Description: null, Type: null, IsActive: null, City: null, InstrumentIds: null, StyleIds: null, TagIds: null
        );

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.UpdateAnnouncementAsync(Guid.NewGuid(), _musicianId, updateDto, CancellationToken.None)
        );
    }

    // ---- DeleteAnnouncement ----

    [Fact]
    public async Task DeleteAnnouncement_ShouldRemoveFromDatabase()
    {
        var created = await CreateDefaultAnnouncement();

        await _announcementService.DeleteAnnouncementAsync(created.Id, _musicianId, CancellationToken.None);

        var exists = await _dbContext.Announcements.AnyAsync(a => a.Id == created.Id);
        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenForbidden()
    {
        var created = await CreateDefaultAnnouncement();
        var fakeMusicianId = Guid.NewGuid();

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementForbiddenException>(() =>
            _announcementService.DeleteAnnouncementAsync(created.Id, fakeMusicianId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.DeleteAnnouncementAsync(Guid.NewGuid(), _musicianId, CancellationToken.None)
        );
    }
}