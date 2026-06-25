using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.EntitiesConfiguration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Country)
            .IsRequired()
            .HasMaxLength(75);
        
        builder.Property(l => l.City)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(l => l.PostalCode)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.HasIndex(l => l.City).IsUnique();
    }
}