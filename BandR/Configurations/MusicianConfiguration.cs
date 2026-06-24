using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.Configurations;

public class MusicianConfiguration : IEntityTypeConfiguration<Musician>
{
    public void Configure(EntityTypeBuilder<Musician> builder)
    {
        builder.ToTable("musicians");

        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Username)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(m => m.Bio)
            .HasMaxLength(1024);

        builder.HasOne(m => m.Location)
            .WithMany()
            .HasForeignKey(m => m.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.AppUser)
            .WithOne()
            .HasForeignKey<Musician>(m => m.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}