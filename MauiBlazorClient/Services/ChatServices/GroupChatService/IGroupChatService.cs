using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices.GroupChatService;

public interface IGroupChatService : IBaseChatService
{
    Task CreateChatAsync(GroupChatRequest chatRequest);
    Task RemoveChatAsync(int chatId);

    Task LeaveGroupAsync(int chatId);
}