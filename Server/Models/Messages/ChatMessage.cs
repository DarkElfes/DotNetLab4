using Server.Models.Chats;

namespace Server.Models.Messages;

public class ChatMessage
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    
    public ApplicationUser? User {get; set;}
    public BaseChat? Chat { get; set; }
}
