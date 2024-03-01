using Server.Models;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services.IServices;

public interface IChatService<TRequest> 
    where TRequest : ChatRequest
{
    Task<BaseChatDTO?> GetChatByIdAsync(int chatId);
    Task<List<BaseChatDTO>> GetChatsByUserAsync(ApplicationUser user);

    Task<BaseChatDTO> UpdateChatAsync(BaseChatDTO chatDTO);
    Task<BaseChatDTO> CreateChatAsync(TRequest chatDTO, ApplicationUser currentUser);
    Task<bool> RemoveChatAsync(int chatId);
}
