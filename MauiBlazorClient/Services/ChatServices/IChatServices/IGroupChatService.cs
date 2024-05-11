using Shared.DTOs.Request.Chat;

namespace MauiBlazorClient.Services.ChatServices.IChatServices;

public interface IGroupChatService : IBaseChatService
{
    Task CreateChatAsync(GroupChatRequest chatRequest);
    Task LeaveFromChatAsync(int chatId);

    Task AddUserAsync(AddUserRequest addUserRequest, int chatId);
}