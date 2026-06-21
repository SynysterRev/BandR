namespace Bandix.Infrastructure.Persistence.Seeds
{
    public class MusicStyleSeeds
    {
        private static readonly DateTime SeedDate = new(2026, 06, 21, 0, 0, 0, DateTimeKind.Utc);

        public static readonly object[] Data =
        {
            // Rock - 10000000
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Hard Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Punk Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Alternative Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Indie Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Metal - 20000000
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "Metal", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Heavy Metal", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Name = "Death Metal", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Name = "Black Metal", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Pop/Mainstream - 30000000
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), Name = "Pop", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"), Name = "Pop Rock", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"), Name = "Electro Pop", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Electronic - 40000000
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"), Name = "Electronic", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000002"), Name = "House", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000003"), Name = "Techno", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000004"), Name = "Dubstep", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000005"), Name = "Trance", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Hip Hop/Rap - 50000000
            new
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000001"), Name = "Hip Hop", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000002"), Name = "Rap", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000003"), Name = "Trap", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Jazz/Blues - 60000000
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000001"), Name = "Jazz", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000002"), Name = "Blues", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000003"), Name = "Soul", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000004"), Name = "Funk", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000005"), Name = "R&B", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Folk/Country - 70000000
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000001"), Name = "Folk", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000002"), Name = "Country", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000003"), Name = "Bluegrass", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Latin - 80000000
            new
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000001"), Name = "Reggae", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000002"), Name = "Ska", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000003"), Name = "Latin", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("80000000-0000-0000-0000-000000000004"), Name = "Salsa", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Classical - 90000000
            new
            {
                Id = Guid.Parse("90000000-0000-0000-0000-000000000001"), Name = "Classical", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("90000000-0000-0000-0000-000000000002"), Name = "Opera", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Other - A0000000
            new
            {
                Id = Guid.Parse("A0000000-0000-0000-0000-000000000001"), Name = "Experimental", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("A0000000-0000-0000-0000-000000000002"), Name = "Ambient", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("A0000000-0000-0000-0000-000000000003"), Name = "World Music", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
        };
    }
}