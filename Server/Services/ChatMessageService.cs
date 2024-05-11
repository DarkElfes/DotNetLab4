using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models.Messages;
using Server.Repositories.IRepositories;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;

namespace Server.Services;

public record MessageService(
    IRepository<ChatMessage> _chatMessageRepo,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager,
    ApplicationDbContext _db
    ) : IMessageService
{
    public async Task<MessageDTO> CreateMessageAsync(MessageRequest messageRequest, ApplicationUser currentUser)
        => _mapper.Map<MessageDTO>(await _chatMessageRepo.CreateAsync(new()
        {
            Message = messageRequest.Message,
            Timestamp = DateTime.UtcNow,

            User = currentUser,
            Chat = await _db.BaseChats
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == messageRequest.ChatId)
        }));
}
