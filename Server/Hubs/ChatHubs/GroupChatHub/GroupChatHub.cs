using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Helpers;
using Server.Models;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Hubs.ChatHubs.GroupChatHub;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GroupChatHub(
    IGroupChatService _chatService,
    IMessageService chatMessageService,
    UserHelper userHelper
    ) : Hub<IGroupChatClient>
{
    private readonly IGroupChatService _chatService = _chatService;
    private readonly IMessageService _chatMessageService = chatMessageService;
    private readonly UserHelper _userHelper = userHelper;

    public override async Task OnConnectedAsync()
    {
        List<BaseChatDTO> chats = await _chatService.GetChatsByUserAsync(await _userHelper.GetCurrentUserAsync(Context?.User));
        await Clients.Caller.ReceiveChats(chats);
    }

    public async Task CreateChatAsync(GroupChatRequest chatRequest)
    {
        BaseChatDTO newChat = await _chatService.CreateChatAsync(chatRequest, await _userHelper.GetCurrentUserAsync(Context?.User));
        await Clients.Caller.ReceiveChat(newChat);
    }
    public async Task RemoveChatAsync(int chatId)
    {
        if (await _chatService.RemoveChatAsync(chatId))
            await Clients.Caller.ReceiveRemovedChatId(chatId);
        else
            await Clients.Caller.ReceiveErrorMessage("Failed to delete this chat.");
    }

    public async Task SendMessageAsync(MessageRequest messageRequest)
    {
        MessageDTO messageDTO = await _chatMessageService.CreateMessageAsync(messageRequest, await _userHelper.GetCurrentUserAsync(Context?.User));

        foreach (UserDTO user in messageDTO!.Chat!.Users)
            await Clients.User(user.Id).ReceiveMessage(messageDTO);
    }


    public async Task LeaveFromChatAsync(int chatId)
    {
        ApplicationUser currentUser = await _userHelper.GetCurrentUserAsync(Context?.User);

        try
        {
            BaseChatDTO chat = await _chatService.LeaveFromChat(chatId, currentUser.Id);
            await Clients.Caller.ReceiveRemovedChatId(chatId);

            foreach (UserDTO user in chat.Users)
                await Clients.User(user.Id).ReceiveChat(chat);
        }
        catch(Exception ex)
        {
            await Clients.Caller.ReceiveErrorMessage(ex.Message);
        }
    } 
    public async Task AddUserToChatAsync(int chatId, string ownerId, string userId)
    {
        throw new NotImplementedException();
    }
    public async Task RemoveUserFromChatAsync(int chatId, string ownerId, string userId)
    {
        throw new NotImplementedException();
    }


}
