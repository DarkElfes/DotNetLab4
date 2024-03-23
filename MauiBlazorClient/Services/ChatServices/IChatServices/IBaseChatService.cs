using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices.IChatServices;

public interface IBaseChatService
{
    List<BaseChatDTO> Chats { get; set; }

    event Action<List<BaseChatDTO>>? OnChatsChanged;

    Task ConnectAsync(string token);
    Task DisconnectAsync();

    Task RemoveChatAsync(int chatId);
    Task SendMessageAsync(MessageRequest messageRequest);
}
