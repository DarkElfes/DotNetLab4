namespace Shared.DTOs.Response.ChatsDTO;

public class BaseChatDTO
{
    public int Id { get; set; }

    public List<UserDTO> Users { get; set; } = [];
    public List<MessageDTO> Messages { get; set; } = [];
}