using System.Reflection;
using BandR.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BandR.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Musician> Musicians { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Style> Styles { get; set; }
    public DbSet<Instrument> Instruments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        builder.Entity<Musician>()
            .HasMany(m => m.Styles)
            .WithMany(s => s.Musicians)
            .UsingEntity(j => j.ToTable("musician_styles"));

        builder.Entity<Musician>()
            .HasMany(m => m.Tags)
            .WithMany(t => t.Musicians)
            .UsingEntity(j => j.ToTable("musician_tags"));

        builder.Entity<Announcement>()
            .HasMany(a => a.Styles)
            .WithMany(s => s.Announcements)
            .UsingEntity(j => j.ToTable("announcement_styles"));

        builder.Entity<Announcement>()
            .HasMany(a => a.Tags)
            .WithMany(t => t.Announcements)
            .UsingEntity(j => j.ToTable("announcement_tags"));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => (e.Entity is BaseEntity || e.Entity is ApplicationUser) &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case BaseEntity baseEntity:
                    if (entry.State == EntityState.Added)
                        baseEntity.CreatedAt = DateTime.UtcNow;
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                    break;

                case ApplicationUser appUser:
                    if (entry.State == EntityState.Added)
                        appUser.CreatedAt = DateTime.UtcNow;
                    appUser.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}