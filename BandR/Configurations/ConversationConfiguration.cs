using BandR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandR.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");
        
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Announcement)
            .WithMany()
            .HasForeignKey(c => c.AnnouncementId)
            .IsRequired(false);
    }
}