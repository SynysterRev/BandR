namespace BandR.DTOs.Conversation;

public record CreateConversationDto(
    Guid? AnnouncementId,
    Guid OtherMusicianId
);