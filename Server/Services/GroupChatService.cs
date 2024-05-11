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
    IChatRepository<GroupChat> _groupChatRepo,
    IMapper _mapper,
    UserManager<ApplicationUser> _userManager
    ) : IGroupChatService
{
    public async Task<GroupChatDTO?> GetChatByIdAsync(int chatId)
        => _mapper.Map<GroupChatDTO>(await _groupChatRepo.GetByIdAsync(chatId));

    public async Task<List<GroupChatDTO>> GetChatListByUserAsync(ApplicationUser user)
        => _mapper.Map<List<GroupChatDTO>>(await _groupChatRepo.GetByUser(user));



    public async Task<GroupChatDTO> UpdateChatAsync(BaseChatDTO chatDTO)
        => _mapper.Map<GroupChatDTO>(await _groupChatRepo.UpdateAsync(_mapper.Map<GroupChat>(chatDTO)));
    public async Task<GroupChatDTO> CreateChatAsync(GroupChatRequest groupChatRequest, ApplicationUser currentUser)
    {
        if (string.IsNullOrWhiteSpace(groupChatRequest.Name))
            throw new ArgumentException("You can't create a group without name");

        List<GroupChat> allChats = await _groupChatRepo.GetAllAsync();

        if (allChats.Any(c => c.Name.Equals(groupChatRequest.Name)))
            throw new ArgumentException("Group by that name already exists");

        GroupChat chat = new()
        {
            Name = groupChatRequest.Name,
            Owner = currentUser,
        };
        chat.Users.Add(currentUser);

        chat = await _groupChatRepo.CreateAsync(chat);

        return _mapper.Map<GroupChatDTO>(chat);
    }
    public async Task<bool> RemoveChatAsync(int chatId)
        => await _groupChatRepo.DeleteAsync(chatId);


    public async Task<GroupChatDTO> LeaveFromChat(int chatId, string userId)
    {
        if (await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
            await _userManager.FindByIdAsync(userId) is ApplicationUser user)
        {
            if (!chat.Users.Remove(user))
                throw new ArgumentException("Current user is not in this chat");

            return _mapper.Map<GroupChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }
    public async Task<GroupChatDTO> AddUserAsync(int chatId, string ownerId, string userEmail)
    {
        if (await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
            await _userManager.FindByEmailAsync(userEmail) is ApplicationUser user)
        {
            if (!ownerId.Equals(chat?.Owner.Id))
                throw new ArgumentException("You don't have permission");

            chat.Users.Add(user);

            return _mapper.Map<GroupChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }
    public async Task<GroupChatDTO> RemoveUserFromChat(int chatId, string ownerId, string userId)
    {
        if (await _groupChatRepo.GetByIdAsync(chatId) is GroupChat chat &&
           await _userManager.FindByIdAsync(userId) is ApplicationUser user)
        {
            if (!ownerId.Equals(chat.Owner.Id))
                throw new ArgumentException("You don't have permission");

            if (!chat.Users.Remove(user))
                throw new ArgumentException("Current user is not in this chat");

            return _mapper.Map<GroupChatDTO>(await _groupChatRepo.UpdateAsync(chat));
        }

        throw new ArgumentNullException("User or chat not exist");
    }

    public Task<List<GroupChatDTO>> GetChatListByUserAsyncDB(ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupChatDTO>> GetChatListByUserCompiled(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}
