using BandR.Entities.Joints;

namespace BandR.Entities;

public class Conversation : BaseEntity
{
    public Guid? AnnouncementId { get; set; }
    public Announcement? Announcement { get; set; }
    
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<MusicianConversation> MusicianConversations { get; set; } = [];
}