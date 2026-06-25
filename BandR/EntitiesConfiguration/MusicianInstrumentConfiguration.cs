using BandR.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.EntitiesConfiguration;

public class MusicianInstrumentConfiguration : IEntityTypeConfiguration<MusicianInstrument>
{
    public void Configure(EntityTypeBuilder<MusicianInstrument> builder)
    {
        builder.ToTable("musician_instruments");
        builder.HasKey(mi => new { mi.MusicianId, mi.InstrumentId });
        
        builder.HasOne(mi => mi.Musician)
            .WithMany(m => m.MusicianInstruments)
            .HasForeignKey(mi => mi.MusicianId);
            
        builder.HasOne(mi => mi.Instrument)
            .WithMany(i => i.MusicianInstruments)
            .HasForeignKey(mi => mi.InstrumentId);
    }
}