namespace Shared.DTOs.Response.ChatsDTO;

public class GroupChatDTO : BaseChatDTO
{
    public string Name { get; set; } = null!;
    public UserDTO Owner { get; set; } = null!;
}

