namespace BandR.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; } =  string.Empty;
    public DateTime SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
    
    public Guid SenderId { get; set; }
    public Musician Sender { get; set; } = null!;
}