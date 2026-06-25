using Bandix.Infrastructure.Persistence.Seeds;
using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.EntitiesConfiguration;

public class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.ToTable("instruments");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(i => i.Name).IsUnique();
        
        builder.HasData(InstrumentSeeds.Data);
    }
}