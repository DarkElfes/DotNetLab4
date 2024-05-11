using Microsoft.AspNetCore.Identity;
using Server.Models.Chats;
using Server.Models.Messages;

namespace Server.Models;

public class ApplicationUser : IdentityUser
{
    //public string ImageUrl { get; set; } = "/images/personal-chat-icon";

    public List<BaseChat> Chats { get; set; } = [];
    public List<ChatMessage> ChatMessages { get; set; } = [];
}
