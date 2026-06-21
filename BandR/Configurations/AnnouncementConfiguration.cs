using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("announcements");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasOne(a => a.Location)
            .WithMany()
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a => a.Musician)
            .WithMany()
            .HasForeignKey(a => a.MusicianId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}