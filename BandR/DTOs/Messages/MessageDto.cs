namespace BandR.DTOs.Messages;

public record MessageDto(
    string Content,
    string SenderName,
    DateTime? ReadAt
);