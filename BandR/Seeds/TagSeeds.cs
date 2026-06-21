namespace Bandix.Infrastructure.Persistence.Seeds;

public static class TagSeeds
{
    private static readonly DateTime SeedDate = new(2026, 06, 21, 0, 0, 0, DateTimeKind.Utc);

    public static readonly object[] Data =
    [
        // Level - 10000000
        // Level - 10000000
        new
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Beginner", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Intermediate", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Advanced", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Professional", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },

// Goal - 20000000
        new
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "Looking for a band", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Looking for a jam session",
            IsValidated = true, CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Name = "Open to collaborations",
            IsValidated = true, CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Name = "Available quickly", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },

// Context - 30000000
        new
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), Name = "Studio", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000002"), Name = "Live / Stage", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000003"), Name = "Rehearsal", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000004"), Name = "Recording", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },

// Playing style - 40000000
        new
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000001"), Name = "Cover", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000002"), Name = "Original compositions", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
        new
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000003"), Name = "Improvisation", IsValidated = true,
            CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
        },
    ];
}