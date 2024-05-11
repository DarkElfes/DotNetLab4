namespace Shared.DTOs.Request.Chat;

public record MessageRequest(
    string Message)
{
    public int ChatId { get; set; }
}
