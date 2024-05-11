using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Helpers;
using Server.Hubs.ChatHubs.IChatClients;
using Server.Models;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Hubs.ChatHubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GroupChatHub(
    IGroupChatService chatService,
    IMessageService chatMessageService,
    UserHelper userHelper
    ) : Hub<IGroupChatClient>
{
    private readonly IGroupChatService _chatService = chatService;
    private readonly IMessageService _chatMessageService = chatMessageService;
    private readonly UserHelper _userHelper = userHelper;

    //private Dictionary<string, List<string>> GroupConnections = [];

    public override async Task OnConnectedAsync()
    {
        ApplicationUser user = await _userHelper.GetCurrentUserAsync(Context?.User);

        List<GroupChatDTO> chats = await _chatService.GetChatListByUserAsync(user);

        /*chats.ForEach(c =>
        {
            Groups.AddToGroupAsync(Context.ConnectionId, c.Name);

            if (!GroupConnections.TryGetValue(c.Name, out List<string> connectedUserId))
                GroupConnections.Add(c.Name, [Context.ConnectionId]);
            else
                connectedUserId.Add(Context.ConnectionId);

            c.CountActiveUsers = connectedUserId.Count;
        });*/


        await Clients.Caller.ReceiveChatList(chats.Cast<BaseChatDTO>().ToList());
    }

    public async Task CreateChatAsync(GroupChatRequest chatRequest)
    {
        try
        {
            BaseChatDTO newChat = await _chatService.CreateChatAsync(chatRequest, await _userHelper.GetCurrentUserAsync(Context?.User));
            await Clients.Caller.ReceiveChat(newChat);
        }
        catch (Exception ex)
        {
            await Clients.Caller.ReceiveErrorMessage(ex.Message);
        }
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
        GroupChatDTO? chat = await _chatService.GetChatByIdAsync(messageRequest.ChatId);

        MessageDTO messageDTO = await _chatMessageService.CreateMessageAsync(messageRequest, await _userHelper.GetCurrentUserAsync(Context?.User));

        messageDTO.Chat?.Users
            .ForEach(async u => await Clients.User(u.Id).ReceiveMessage(messageDTO));
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
        catch (Exception ex)
        {
            await Clients.Caller.ReceiveErrorMessage(ex.Message);
        }
    }
    public async Task AddUserAsync(AddUserRequest addUserRequest, int chatId)
    {
        try
        {
            ApplicationUser currentUser = await _userHelper.GetCurrentUserAsync(Context?.User);

            GroupChatDTO chat = await _chatService.AddUserAsync(chatId, currentUser.Id, addUserRequest.UserEmail);

            foreach (UserDTO user in chat.Users)
                await Clients.User(user.Id).ReceiveChat(chat);
        }
        catch (Exception ex)
        {
            await Clients.Caller.ReceiveErrorMessage(ex.Message);
        }
    }
    public async Task RemoveUserFromChatAsync(int chatId, string ownerId, string userId)
    {
        throw new NotImplementedException();
    }


}
