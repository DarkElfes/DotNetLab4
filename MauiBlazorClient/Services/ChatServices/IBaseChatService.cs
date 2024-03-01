using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices;

public interface IBaseChatService
{
    List<BaseChatDTO> Chats { get; set; }

    event Action<List<BaseChatDTO>>? OnChatsChanged;

    Task ConnectAsync(string token);
    Task DisconnectAsync();

    Task SendMessageAsync(MessageRequest messageRequest);
}
