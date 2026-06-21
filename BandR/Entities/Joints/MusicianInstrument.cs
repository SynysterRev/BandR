namespace BandR.Entities.Joints;

public class MusicianInstrument
{
    public Guid MusicianId { get; set; }
    public Musician Musician { get; set; } = null!;
    
    public Guid InstrumentId { get; set; }
    public Instrument Instrument { get; set; } = null!;
}