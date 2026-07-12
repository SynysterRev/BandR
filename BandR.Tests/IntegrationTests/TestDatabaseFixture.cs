using BandR.Data;
using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BandR.Tests.IntegrationTests;

public class TestDatabaseFixture: IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    public ApplicationDbContext DbContext = null!;
    public readonly Guid MusicianId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public Guid AppUserId;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        DbContext = new ApplicationDbContext(options);
        await DbContext.Database.MigrateAsync();

        var defaultLocation = new Location
        {
            Id = Guid.NewGuid(),
            City = "London",
            CreatedAt = DateTime.UtcNow
        };
        await DbContext.Locations.AddAsync(defaultLocation);

        var appUser = new ApplicationUser
        {
            Id = MusicianId,
            UserName = "testuser",
            Email = "test@test.com",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        await DbContext.Users.AddAsync(appUser);
    
        await DbContext.SaveChangesAsync(); 
        
        AppUserId = appUser.Id;

        await DbContext.Musicians.AddAsync(new Musician
        {
            Id = MusicianId,
            AppUserId = MusicianId,
            Username = "TestMusician",
            LocationId = defaultLocation.Id,
            CreatedAt = DateTime.UtcNow
        });
        await DbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}