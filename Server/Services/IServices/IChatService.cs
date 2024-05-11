using Server.Models;
using Server.Models.Chats;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services.IServices;

public interface IChatService<TRequest, TResponse> 
    where TRequest : ChatRequest
    where TResponse : BaseChatDTO
{
    Task<TResponse?> GetChatByIdAsync(int chatId);
    Task<List<TResponse>> GetChatListByUserAsync(ApplicationUser user);

    Task<TResponse> UpdateChatAsync(BaseChatDTO chatDTO);
    Task<TResponse> CreateChatAsync(TRequest chatDTO, ApplicationUser currentUser);
    Task<bool> RemoveChatAsync(int chatId);
}
