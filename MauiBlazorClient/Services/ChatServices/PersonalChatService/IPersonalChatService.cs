using Microsoft.AspNetCore.SignalR.Client;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices.PersonalChatService;

public interface IPersonalChatService : IBaseChatService
{
    Task CreateChatAsync(PersonalChatRequest chatRequest);
    Task RemoveChatAsync(int chatId);
}