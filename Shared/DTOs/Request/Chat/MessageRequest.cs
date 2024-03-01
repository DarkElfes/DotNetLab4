namespace Shared.DTOs.Request.Chat;

public record MessageRequest(
    string Message,
    DateTime Timestamp
    )
{
    public int ChatId { get; set; }
}
