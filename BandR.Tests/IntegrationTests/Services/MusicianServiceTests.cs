using BandR.Data;
using BandR.DTOs.Musicians;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Services;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BandR.Tests.IntegrationTests.Services;

public sealed class MusicianServiceTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    private ApplicationDbContext _dbContext = null!;
    private MusicianService _musicianService = null!;
    private readonly Guid _appUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        await _dbContext.Database.MigrateAsync();

        await _dbContext.Users.AddAsync(new ApplicationUser
        {
            Id = _appUserId,
            UserName = "testuser",
            Email = "test@test.com",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        });
        await _dbContext.SaveChangesAsync();

        _musicianService = new MusicianService(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    // ---- Helpers ----

    private async Task<MusicianDto> CreateDefaultMusician(
        string username = "TestMusician",
        string city = "Montpellier")
    {
        var dto = new CreateMusicianDto(
            Username: username,
            City: city,
            InstrumentIds: [],
            StyleIds: [],
            TagIds: [],
            Bio: null
        );
        return await _musicianService.CreateMusician(dto, _appUserId, CancellationToken.None);
    }

    // ---- CreateMusician ----

    [Fact]
    public async Task CreateMusician_ShouldReturnMusicianDto()
    {
        var result = await CreateDefaultMusician();

        Assert.NotNull(result);
        Assert.Equal("TestMusician", result.Username);
        Assert.Equal("Montpellier", result.City);
    }

    [Fact]
    public async Task CreateMusician_ShouldReuseExistingLocation()
    {
        await CreateDefaultMusician(username: "Musician1", city: "Montpellier");
        await CreateDefaultMusician(username: "Musician2", city: "Montpellier");

        var locationCount = await _dbContext.Locations
            .CountAsync(l => l.City.ToLower() == "montpellier");

        Assert.Equal(1, locationCount);
    }

    [Fact]
    public async Task CreateMusician_WithInstruments_ShouldAttachThem()
    {
        var instrument = new Instrument
        {
            Name = "Test Guitar",
            IsValidated = true,
            CreatedAt = DateTime.UtcNow
        };
        _dbContext.Instruments.Add(instrument);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateMusicianDto(
            Username: "Guitarist",
            City: "Paris",
            InstrumentIds: [instrument.Id],
            StyleIds: [],
            TagIds: [],
            Bio: null
        );

        var result = await _musicianService.CreateMusician(dto, _appUserId, CancellationToken.None);

        Assert.Single(result.Instruments);
        Assert.Equal("Test Guitar", result.Instruments[0]);
    }

    // ---- GetMusicianById ----

    [Fact]
    public async Task GetMusicianById_ShouldReturnMusician()
    {
        var created = await CreateDefaultMusician();

        var result = await _musicianService.GetMusicianById(created.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("TestMusician", result.Username);
    }

    [Fact]
    public async Task GetMusicianById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.GetMusicianById(Guid.NewGuid(), CancellationToken.None)
        );
    }

    // ---- GetMusicians ----

    [Fact]
    public async Task GetMusicians_ShouldReturnAllMusicians()
    {
        await CreateDefaultMusician(username: "Musician1");
        await CreateDefaultMusician(username: "Musician2");
        var countAfter = await _dbContext.Musicians.CountAsync();

        var result = await _musicianService.GetMusicians(CancellationToken.None);

        Assert.Equal(countAfter, result.Count);
    }

    [Fact]
    public async Task GetMusicians_ShouldReturnEmpty_WhenNoMusicians()
    {
        var result = await _musicianService.GetMusicians(CancellationToken.None);

        Assert.Empty(result);
    }

    // ---- UpdateMusician ----

    [Fact]
    public async Task UpdateMusician_ShouldUpdateUsername()
    {
        var created = await CreateDefaultMusician();

        var updateDto = new UpdateMusicianDto(
            Username: "UpdatedUsername",
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result = await _musicianService.UpdateMusician(created.Id, updateDto, _appUserId, CancellationToken.None);

        Assert.Equal("UpdatedUsername", result.Username);
    }

    [Fact]
    public async Task UpdateMusician_ShouldThrow_WhenNotFound()
    {
        var updateDto = new UpdateMusicianDto(
            Username: "Whatever",
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.UpdateMusician(Guid.NewGuid(), updateDto, _appUserId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateMusician_ShouldThrow_WhenForbidden()
    {
        var created = await CreateDefaultMusician();
        var otherUserId = Guid.NewGuid();

        var updateDto = new UpdateMusicianDto(
            Username: "Hacked",
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        await Assert.ThrowsAsync<MusicianException.MusicianForbiddenException>(() =>
            _musicianService.UpdateMusician(created.Id, updateDto, otherUserId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateMusician_ShouldNotUpdateNull_Fields()
    {
        var created = await CreateDefaultMusician(username: "OriginalName");

        var updateDto = new UpdateMusicianDto(
            Username: null,
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result = await _musicianService.UpdateMusician(created.Id, updateDto, _appUserId, CancellationToken.None);

        Assert.Equal("OriginalName", result.Username);
    }

    // ---- DeleteMusician ----

    [Fact]
    public async Task DeleteMusician_ShouldRemoveMusician()
    {
        var created = await CreateDefaultMusician();

        await _musicianService.DeleteMusician(created.Id, CancellationToken.None);

        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.GetMusicianById(created.Id, CancellationToken.None)
        );
    }

    [Fact]
    public async Task DeleteMusician_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.DeleteMusician(Guid.NewGuid(), CancellationToken.None)
        );
    }
}