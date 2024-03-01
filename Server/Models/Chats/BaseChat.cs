using Server.Models.Messages;

namespace Server.Models.Chats;

public class BaseChat 
{
    public int Id { get; set; }

    public List<ApplicationUser> Users { get; set; } = new();
    public List<ChatMessage> Messages { get; set; } = new();
}



