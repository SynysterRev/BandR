namespace Bandix.Infrastructure.Persistence.Seeds
{
    public static class InstrumentSeeds
    {
        private static readonly DateTime SeedDate = new(2026, 06, 21, 0, 0, 0, DateTimeKind.Utc);

        public static readonly object[] Data =
        {
            // Strings - 10000000
            // Strings - 10000000
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Acoustic Guitar", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Electric Guitar", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Bass Guitar", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Violin", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Cello", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Ukulele", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Keyboards - 20000000
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "Piano", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Keyboard", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Name = "Synthesizer", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Name = "Organ", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Percussion - 30000000
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), Name = "Drums", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"), Name = "Percussion", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"), Name = "Cajon", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000004"), Name = "Djembe", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Winds - 40000000
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"), Name = "Saxophone", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000002"), Name = "Trumpet", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000003"), Name = "Flute", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000004"), Name = "Clarinet", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000005"), Name = "Trombone", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000006"), Name = "Harmonica", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Vocals - 50000000
            new
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000001"), Name = "Vocals", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Electronic/DJ - 60000000
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000001"), Name = "DJ Controller", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000002"), Name = "Turntables", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000003"), Name = "Drum Machine", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },

// Other - 70000000
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000001"), Name = "Accordion", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000002"), Name = "Banjo", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000003"), Name = "Mandolin", IsValidated = true,
                CreatedAt = SeedDate, UpdatedAt = (DateTime?)null
            },
        };
    }
}