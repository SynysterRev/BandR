using BandR.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.EntitiesConfiguration;

public class AnnouncementInstrumentConfiguration : IEntityTypeConfiguration<AnnouncementInstrument>
{
    public void Configure(EntityTypeBuilder<AnnouncementInstrument> builder)
    {
        builder.ToTable("announcement_instruments");
        builder.HasKey(ai => new { ai.AnnouncementId, ai.InstrumentId });
        
        builder.HasOne(ai => ai.Announcement)
            .WithMany(m => m.AnnouncementInstruments)
            .HasForeignKey(ai => ai.AnnouncementId);
            
        builder.HasOne(ai => ai.Instrument)
            .WithMany(i => i.AnnouncementInstruments)
            .HasForeignKey(ai => ai.InstrumentId);
    }
}