using BandR.Common;
using BandR.Data;
using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Exceptions;
using BandR.Services;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace BandR.Tests.IntegrationTests.Services;

public sealed class AnnouncementServiceTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    private ApplicationDbContext _dbContext = null!;
    private AnnouncementService _announcementService = null!;
    private readonly Guid _musicianId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        await _dbContext.Database.MigrateAsync();

        // 1. On crée une localisation par défaut (requise par le Musician)
        var defaultLocation = new Location
        {
            Id = Guid.NewGuid(),
            City = "Montpellier",
            CreatedAt = DateTime.UtcNow
        };
        await _dbContext.Locations.AddAsync(defaultLocation);

        // 2. On crée l'utilisateur Identity
        var appUser = new ApplicationUser
        {
            Id = _musicianId,
            UserName = "testuser",
            Email = "test@test.com",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        await _dbContext.Users.AddAsync(appUser);
    
        // On sauvegarde pour générer les clés en base
        await _dbContext.SaveChangesAsync(); 

        // 3. On crée le musicien lié au User ET à la Location
        await _dbContext.Musicians.AddAsync(new Musician
        {
            Id = _musicianId,
            AppUserId = _musicianId,
            Username = "TestMusician",
            LocationId = defaultLocation.Id, // <-- La pièce manquante !
            CreatedAt = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();

        _announcementService = new AnnouncementService(_dbContext);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    // ---- Helpers ----

    private async Task<AnnouncementDto> CreateDefaultAnnouncement(
        string title = "Cherche Bassiste",
        string city = "Montpellier",
        Guid? musicianId = null)
    {
        var dto = new CreateAnnouncementDto(
            Title: title,
            Description: "Pour groupe de Rock alternatif",
            City: city,
            Type: AnnouncementType.LookingForMusician,
            InstrumentIds: [],
            StyleIds: [],
            TagIds: []
        );

        return await _announcementService.CreateAnnouncement(dto, musicianId ?? _musicianId, CancellationToken.None);
    }

    // ---- CreateAnnouncement ----

    [Fact]
    public async Task CreateAnnouncement_ShouldReturnAnnouncementDto()
    {
        var result = await CreateDefaultAnnouncement();

        Assert.NotNull(result);
        Assert.Equal("Cherche Bassiste", result.Title);
        Assert.Equal("Montpellier", result.City);
    }

    [Fact]
    public async Task CreateAnnouncement_ShouldReuseExistingLocation()
    {
        await CreateDefaultAnnouncement(title: "Annonce 1", city: "Lyon");
        await CreateDefaultAnnouncement(title: "Annonce 2", city: "Lyon");

        var locationCount = await _dbContext.Locations
            .CountAsync(l => l.City.ToLower() == "lyon");

        Assert.Equal(1, locationCount);
    }

    // ---- GetAnnouncementById ----

    [Fact]
    public async Task GetAnnouncementById_ShouldReturnAnnouncement()
    {
        var created = await CreateDefaultAnnouncement();

        var result = await _announcementService.GetAnnouncementById(created.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Cherche Bassiste", result.Title);
    }

    [Fact]
    public async Task GetAnnouncementById_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.GetAnnouncementById(Guid.NewGuid(), CancellationToken.None)
        );
    }

    // ---- GetAnnouncements (Filtré/Actif) ----

    [Fact]
    public async Task GetAnnouncements_ShouldReturnOnlyActiveAnnouncements()
    {
        // Création d'une annonce active par défaut
        await CreateDefaultAnnouncement(title: "Active 1");
        
        // Création d'une annonce et passage forcé à inactif en BDD
        var inactive = await CreateDefaultAnnouncement(title: "Inactive 1");
        var entity = await _dbContext.Announcements.FindAsync(inactive.Id);
        entity!.IsActive = false;
        await _dbContext.SaveChangesAsync();

        var filter = new AnnouncementQueryFilter { PageNumber = 1, PageSize = 10 };
        var result = await _announcementService.GetAnnouncements(filter, CancellationToken.None);

        Assert.Contains(result.Data, a => a.Title == "Active 1");
        Assert.DoesNotContain(result.Data, a => a.Title == "Inactive 1");
    }

    // ---- GetAnnouncementsForMusician ----

    [Fact]
    public async Task GetAnnouncementsForMusician_ShouldReturnOnlyMusicianAnnouncements()
    {
        // Récupère l'ID de la localisation créée dans InitializeAsync
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
            LocationId = existingLocation.Id, // <-- On lui passe aussi la localisation
            Username = "Other", 
            CreatedAt = DateTime.UtcNow 
        });
        await _dbContext.SaveChangesAsync();

        // Reste du test identique...
        await CreateDefaultAnnouncement(title: "Ma super annonce", musicianId: _musicianId);
        await CreateDefaultAnnouncement(title: "Annonce de l'autre", musicianId: otherMusicianId);

        var filter = new AnnouncementQueryFilter { PageNumber = 1, PageSize = 10 };
        var result = await _announcementService.GetAnnouncementsForMusician(_musicianId, filter, CancellationToken.None);

        Assert.Single(result.Data);
        Assert.Equal("Ma super annonce", result.Data.First().Title);
    }

    // ---- UpdateAnnouncement ----

    [Fact]
    public async Task UpdateAnnouncement_ShouldUpdateFields()
    {
        var created = await CreateDefaultAnnouncement();

        var updateDto = new UpdateAnnouncementDto(
            Title: "Titre Modifié",
            Description: "Nouvelle description",
            Type: null,
            IsActive: false,
            City: "Paris",
            InstrumentIds: null,
            StyleIds: null,
            TagIds: null
        );

        var result = await _announcementService.UpdateAnnouncement(created.Id, _musicianId, updateDto, CancellationToken.None);

        Assert.Equal("Titre Modifié", result.Title);
        Assert.Equal("Nouvelle description", result.Description);
        Assert.Equal("Paris", result.City);
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
            _announcementService.UpdateAnnouncement(created.Id, fakeMusicianId, updateDto, CancellationToken.None)
        );
    }

    [Fact]
    public async Task UpdateAnnouncement_ShouldThrow_WhenNotFound()
    {
        var updateDto = new UpdateAnnouncementDto(
            Title: "Oups", Description: null, Type: null, IsActive: null, City: null, InstrumentIds: null, StyleIds: null, TagIds: null
        );

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.UpdateAnnouncement(Guid.NewGuid(), _musicianId, updateDto, CancellationToken.None)
        );
    }

    // ---- DeleteAnnouncement ----

    [Fact]
    public async Task DeleteAnnouncement_ShouldRemoveFromDatabase()
    {
        var created = await CreateDefaultAnnouncement();

        await _announcementService.DeleteAnnouncement(created.Id, _musicianId, CancellationToken.None);

        var exists = await _dbContext.Announcements.AnyAsync(a => a.Id == created.Id);
        Assert.False(exists);
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenForbidden()
    {
        var created = await CreateDefaultAnnouncement();
        var fakeMusicianId = Guid.NewGuid();

        await Assert.ThrowsAsync<AnnouncementException.AnnouncementForbiddenException>(() =>
            _announcementService.DeleteAnnouncement(created.Id, fakeMusicianId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task DeleteAnnouncement_ShouldThrow_WhenNotFound()
    {
        await Assert.ThrowsAsync<AnnouncementException.AnnouncementNotFoundException>(() =>
            _announcementService.DeleteAnnouncement(Guid.NewGuid(), _musicianId, CancellationToken.None)
        );
    }
}