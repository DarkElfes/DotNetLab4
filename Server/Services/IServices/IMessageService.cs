using Server.Models;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;

namespace Server.Services.IServices;

public interface IMessageService
{
    Task<MessageDTO> CreateMessageAsync(MessageRequest chatMessageRequest, ApplicationUser currentUser);
}
