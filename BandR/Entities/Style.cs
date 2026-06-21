using BandR.Entities.Joints;

namespace BandR.Entities;

public class Style : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsValidated { get; set; } = true;
    
    public ICollection<Musician> Musicians { get; set; } = [];
    public ICollection<Announcement> Announcements { get; set; } = [];
}
