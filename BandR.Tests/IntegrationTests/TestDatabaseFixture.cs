using BandR.Data;
using BandR.DTOs.Musicians;
using BandR.Entities;
using BandR.Services;
using BandR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BandR.Tests.IntegrationTests;

public class TestDatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    public ApplicationDbContext DbContext = null!;
    public string ConnectionString => _postgres.GetConnectionString();
    public Guid AppUserId;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        DbContext = new ApplicationDbContext(options);
        await DbContext.Database.MigrateAsync();

        await SeedDefaultDataAsync();
    }

    public async Task SeedDefaultDataAsync()
    {
        var defaultLocation = new Location
        {
            Id = Guid.NewGuid(),
            City = "London",
            CreatedAt = DateTime.UtcNow
        };
        await DbContext.Locations.AddAsync(defaultLocation);

        var appUser = new ApplicationUser
        {
            UserName = "testuser",
            Email = "test@test.com",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        await DbContext.Users.AddAsync(appUser);
        await DbContext.SaveChangesAsync();

        AppUserId = appUser.Id;
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }
    
    public async Task<MusicianDto> CreateDefaultMusician(
        string username = "TestMusician",
        string city = "Montpellier",
        Guid? customUserId = null)
    {
        var targetUserId = customUserId ?? AppUserId;

        if (customUserId.HasValue)
        {
            var appUser = new ApplicationUser
            {
                Id = customUserId.Value,
                UserName = $"user_{Guid.NewGuid().ToString()[..8]}",
                Email = $"{Guid.NewGuid()}@test.com",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            await DbContext.Users.AddAsync(appUser);
            await DbContext.SaveChangesAsync();
        }

        var dto = new CreateMusicianDto(
            Username: username,
            City: city,
            InstrumentIds: [],
            StyleIds: [],
            TagIds: [],
            Bio: null
        );
        var musicianService = new MusicianService(DbContext);
        return await musicianService.CreateMusicianAsync(dto, targetUserId, CancellationToken.None);
    }
}