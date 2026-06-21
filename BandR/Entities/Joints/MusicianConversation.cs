namespace BandR.Entities.Joints;

public class MusicianConversation
{
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
    
    public Guid MusicianId { get; set; }
    public Musician Musician { get; set; } = null!;
    
    public DateTime JoinedAt { get; set; }
}