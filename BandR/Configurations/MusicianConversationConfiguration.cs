using BandR.Entities.Joints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.Configurations;

public class MusicianConversationConfiguration : IEntityTypeConfiguration<MusicianConversation>
{
    public void Configure(EntityTypeBuilder<MusicianConversation> builder)
    {
        builder.ToTable("musician_conversation");
        builder.HasKey(mc => new { mc.MusicianId, mc.ConversationId });
        
        builder.HasOne(mc => mc.Musician)
            .WithMany(m => m.MusicianConversations)
            .HasForeignKey(mc => mc.MusicianId);
            
        builder.HasOne(mc => mc.Conversation)
            .WithMany(c => c.MusicianConversations)
            .HasForeignKey(mc => mc.ConversationId);
    }
}