using BandR.DTOs.Messages;

namespace BandR.DTOs.Conversation;

public record ConversationDto(
    Guid Id,
    List<MessageDto> Messages,
    Guid? AnnouncementId,
    bool IsActive
);
