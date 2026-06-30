using BandR.Entities.Joints;

namespace BandR.Entities;

public enum AnnouncementType
{
    LookingForMusician,
    LookingForBand
}

public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public AnnouncementType Type { get; set; }
    public Guid MusicianId { get; set; } = Guid.Empty;
    public Musician Musician { get; set; } = null!;
    public Guid LocationId { get; set; } = Guid.Empty;
    public Location Location { get; set; } = null!;
    public ICollection<Style> Styles { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<AnnouncementInstrument> AnnouncementInstruments { get; set; } = [];
}