using MauiBlazorClient.Services.ChatServices.IChatServices;
using Shared.DTOs.Request.Chat;

namespace MauiBlazorClient.Services.ChatServices.IChatServices;

public interface IPersonalChatService : IBaseChatService
{
    Task CreateChatAsync(PersonalChatRequest chatRequest);
}