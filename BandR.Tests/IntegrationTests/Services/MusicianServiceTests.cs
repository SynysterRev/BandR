using BandR.Data;
using BandR.DTOs.Musicians;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Services;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace BandR.Tests.IntegrationTests.Services;

public sealed class MusicianServiceTests : IClassFixture<TestDatabaseFixture>, IAsyncLifetime
{
    private readonly TestDatabaseFixture _fixture;
    private readonly IMusicianService _musicianService;
    private readonly ApplicationDbContext _dbContext;
    private Guid _appUserId;

    public MusicianServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dbContext = fixture.DbContext;
        _musicianService = new MusicianService(_dbContext);
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
    
        _appUserId = _fixture.AppUserId;
    }

    public Task DisposeAsync() => Task.CompletedTask;
    // ---- Helpers ----

    // ---- CreateMusician ----

    [Fact]
    public async Task CreateMusician_ShouldReturnMusicianDto()
    {
        var result = await _fixture.CreateDefaultMusician();

        Assert.NotNull(result);
        Assert.Equal("TestMusician", result.Username);
        Assert.Equal("Montpellier", result.City);
    }

    [Fact]
    public async Task CreateMusician_ShouldReuseExistingLocation()
    {
        await _fixture.CreateDefaultMusician(username: "Musician1", city: "Montpellier");
        await _fixture.CreateDefaultMusician(username: "Musician2", city: "Montpellier");

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

        var result = await _musicianService.CreateMusicianAsync(dto, _appUserId, CancellationToken.None);

        Assert.Single(result.Instruments);
        Assert.Equal("Test Guitar", result.Instruments[0]);
    }

    // ---- GetMusicianById ----

    [Fact]
    public async Task GetMusicianById_ShouldReturnMusician()
    {
        var created = await _fixture.CreateDefaultMusician();

        var result = await _musicianService.GetMusicianByIdAsync(created.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("TestMusician", result.Username);
    }

    [Fact]
    public async Task GetMusicianById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.GetMusicianByIdAsync(Guid.NewGuid(), CancellationToken.None)
        );
    }

    // ---- GetMusicians ----

    [Fact]
    public async Task GetMusicians_ShouldReturnAllMusicians()
    {
        await _fixture.CreateDefaultMusician(username: "Musician1");
        await _fixture.CreateDefaultMusician(username: "Musician2");
        var countAfter = await _dbContext.Musicians.CountAsync();

        var result = await _musicianService.GetMusiciansAsync(CancellationToken.None);

        Assert.Equal(countAfter, result.Count);
    }

    [Fact]
    public async Task GetMusicians_ShouldReturnEmpty_WhenNoMusicians()
    {
        var result = await _musicianService.GetMusiciansAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    // ---- UpdateMusician ----

    [Fact]
    public async Task UpdateMusician_ShouldUpdateUsername()
    {
        var created = await _fixture.CreateDefaultMusician();

        var updateDto = new UpdateMusicianDto(
            Username: "UpdatedUsername",
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result =
            await _musicianService.UpdateMusicianAsync(created.Id, updateDto, _appUserId, CancellationToken.None);

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
            _musicianService.UpdateMusicianAsync(Guid.NewGuid(), updateDto, _appUserId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateMusician_ShouldThrow_WhenForbidden()
    {
        var created = await _fixture.CreateDefaultMusician();
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
            _musicianService.UpdateMusicianAsync(created.Id, updateDto, otherUserId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateMusician_ShouldNotUpdateNull_Fields()
    {
        var created = await _fixture.CreateDefaultMusician(username: "OriginalName");

        var updateDto = new UpdateMusicianDto(
            Username: null,
            City: null,
            Bio: null,
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result =
            await _musicianService.UpdateMusicianAsync(created.Id, updateDto, _appUserId, CancellationToken.None);

        Assert.Equal("OriginalName", result.Username);
    }

    // ---- DeleteMusician ----

    [Fact]
    public async Task DeleteMusician_ShouldRemoveMusician()
    {
        var created = await _fixture.CreateDefaultMusician();

        await _musicianService.DeleteMusicianAsync(created.Id, CancellationToken.None);

        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.GetMusicianByIdAsync(created.Id, CancellationToken.None)
        );
    }

    [Fact]
    public async Task DeleteMusician_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<MusicianException.MusicianNotFoundException>(() =>
            _musicianService.DeleteMusicianAsync(Guid.NewGuid(), CancellationToken.None)
        );
    }
}