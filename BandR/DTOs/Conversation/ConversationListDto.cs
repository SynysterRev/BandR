namespace BandR.DTOs.Conversation;

public record ConversationListDto(
    Guid Id,
    Guid? AnnouncementId,
    bool IsActive,
    Guid OtherMusicianId,
    string OtherMusicianUsername,
    string? LastMessagePreview,
    DateTime? LastMessageSentAt
);
