using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Server.Models;
using Server.Models.Chats;
using Server.Repositories.IRepositories;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services;

public record PersonalChatService(
    IChatRepository<PersonalChat> _personalChatRepo,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager
    ) : IChatService<PersonalChatRequest, PersonalChatDTO>
{
    public async Task<PersonalChatDTO?> GetChatByIdAsync(int chatId)
        => _mapper.Map<PersonalChatDTO>(await _personalChatRepo.GetByIdAsync(chatId));
    public async Task<List<PersonalChatDTO>> GetChatListByUserAsync(ApplicationUser user)
        => _mapper.Map<List<PersonalChatDTO>>(await _personalChatRepo.GetByUser(user));

    public async Task<List<PersonalChatDTO>> GetChatListByUserAsyncDB(ApplicationUser user)
    {
        List<PersonalChat> allChats = await _personalChatRepo.GetByUser(user);

        return _mapper.Map<List<PersonalChatDTO>>(allChats);
    }

    public async Task<PersonalChatDTO> UpdateChatAsync(BaseChatDTO chatDTO)
        => _mapper.Map<PersonalChatDTO>(await _personalChatRepo.UpdateAsync(_mapper.Map<PersonalChat>(chatDTO)));
    public async Task<PersonalChatDTO> CreateChatAsync(PersonalChatRequest personalChatRequest, ApplicationUser currentUser)
    {
        ApplicationUser? otherUser = await _userManager.FindByEmailAsync(personalChatRequest.Email);

        if (otherUser is null)
            throw new ArgumentException("User with this email not exist");
        if (otherUser == currentUser)
            throw new ArgumentException("You can't create a chat with yourself");

        List<PersonalChat> allChats = await _personalChatRepo.GetAllAsync();

        bool isExistChat = allChats.Any(c => c.Users.Any(u => u == currentUser) && c.Users.Any(u => u == otherUser));

        if (isExistChat)
            throw new ArgumentException("You already have a chat with this user");

        PersonalChat chat = new();
        chat.Users.Add(currentUser);
        chat.Users.Add(otherUser);

        chat = await _personalChatRepo.CreateAsync(chat);

        return _mapper.Map<PersonalChatDTO>(chat);
    }
    public async Task<bool> RemoveChatAsync(int chatId)
        => await _personalChatRepo.DeleteAsync(chatId);
}
