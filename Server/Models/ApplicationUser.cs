using Microsoft.AspNetCore.Identity;
using Server.Models.Chats;
using Server.Models.Messages;

namespace Server.Models;

public class ApplicationUser : IdentityUser
{
    public List<BaseChat> Chats { get; set; } = new();

    public List<ChatMessage> ChatMessages { get; set; } = new();
}
