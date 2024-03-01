using Shared.DTOs.Response.ChatsDTO;

namespace Shared.DTOs.Response;

public class MessageDTO
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }

    public UserDTO? User { get; set; }
    public BaseChatDTO? Chat { get; set; }
}
