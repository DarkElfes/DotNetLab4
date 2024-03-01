namespace Shared.DTOs.Response.ChatsDTO;

public class GroupChatDTO : BaseChatDTO
{
    public string? Name { get; set; }
    public UserDTO? OwnerUser { get; set; }
}

