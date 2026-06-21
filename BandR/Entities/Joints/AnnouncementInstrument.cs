namespace BandR.Entities.Joints;

public class AnnouncementInstrument
{
    public Guid AnnouncementId { get; set; }
    public Announcement Announcement { get; set; } = null!;
    
    public Guid InstrumentId { get; set; }
    public Instrument Instrument { get; set; } = null!;
}