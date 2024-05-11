using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.IServices;

internal interface IChatService
{
    event Action<List<BaseChatDTO>>? OnChatsChanged;
    event Action<BaseChatDTO?>? OnCurrentChatChanged;

    BaseChatDTO? CurrentChat { get; }
    List<BaseChatDTO> Chats { get; }


    Task ConnectAsync();
    Task DisconnectAsync();

    Task SendMessageAsync(MessageRequest message);


    void OpenChat(BaseChatDTO chatDTO);
}
