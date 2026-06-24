using BandR.Entities.Joints;

namespace BandR.Entities;

public class Musician : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public Guid AppUserId { get; set; }
    public ApplicationUser AppUser { get; set; } = null!;
    
    public ICollection<MusicianInstrument> MusicianInstruments { get; set; } = [];
    public ICollection<Style> Styles { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
    
    public ICollection<MusicianConversation> MusicianConversations { get; set; } = [];
}