using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Server.Models;
using Server.Models.Chats;
using Server.Repositories.IRepositories;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Services;

public record GroupChatService(
    IRepository<GroupChat> _groupChatRepo,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager
    ) : IGroupChatService
{
    public async Task<BaseChatDTO?> GetChatByIdAsync(int chatId)
        => _mapper.Map<GroupChatDTO>(await _groupChatRepo.GetByIdAsync(chatId));
    public async Task<List<BaseChatDTO>> GetChatsByUserAsync(ApplicationUser user)
    {
        List<GroupChat> allChats = await _groupChatRepo.GetAllAsync();

        List<GroupChatDTO> groupChatDTOs = _mapper.Map<List<GroupChatDTO>>(allChats.Where(c => c.Users.Any(u => u.Id == user.Id)));
        return groupChatDTOs.Cast<BaseChatDTO>().ToList();
    }


    public async Task<BaseChatDTO> UpdateChatAsync(BaseChatDTO chatDTO)
        => _mapper.Map<GroupChatDTO>(await _groupChatRepo.UpdateAsync(_mapper.Map<GroupChat>(chatDTO)));
    public async Task<BaseChatDTO> CreateChatAsync(GroupChatRequest chatDTO, ApplicationUser currentUser)
    {
        GroupChat chat = _mapper.Map<GroupChat>(chatDTO);

        List<GroupChat> allChats = await _groupChatRepo.GetAllAsync();

        if (allChats.Any(c => c.Name.Equals(chatDTO.Name)))
            throw new ArgumentException("Group chat by that name already exists");

        chat.Users.Add(currentUser);

        chat = await _groupChatRepo.CreateAsync(chat);

        return _mapper.Map<GroupChatDTO>(chat);
    }
    public async Task<bool> RemoveChatAsync(int chatId)
        => await _groupChatRepo.DeleteAsync(chatId);


    public async Task<BaseChatDTO> LeaveFromChat(int chatId, string userId)
    {
        if (await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
            await _userManager.FindByIdAsync(userId) is ApplicationUser user)
        {
            if (!chat.Users.Remove(user))
                throw new ArgumentException("Current user is not in this chat");

            return _mapper.Map<BaseChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }
    public async Task<BaseChatDTO> AddUserToChat(int chatId, string ownerId, string userId)
    {
        if(await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
            await _userManager.FindByIdAsync(userId) is ApplicationUser user)
        {
            if (!ownerId.Equals(chat.Owner.Id))
                throw new ArgumentException("You don't have permission");

            chat.Users.Add(user);

            return _mapper.Map<BaseChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }
    public async Task<BaseChatDTO> RemoveUserFromChat(int chatId, string ownerId, string userId)
    {
        if (await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
           await _userManager.FindByIdAsync(userId) is ApplicationUser user)
        {
            if (!ownerId.Equals(chat.Owner.Id))
                throw new ArgumentException("You don't have permission");

            if (!chat.Users.Remove(user))
                throw new ArgumentException("Current user is not in this chat");

            return _mapper.Map<BaseChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }
}
