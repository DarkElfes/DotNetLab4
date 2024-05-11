using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Helpers;
using Server.Hubs.ChatHubs.IChatClients;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Hubs.ChatHubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PersonalChatHub(
    IChatService<PersonalChatRequest, PersonalChatDTO> chatService,
    IMessageService chatMessageService,
    UserHelper userHelper
    ) : Hub<IPersonalChatClient>
{
    private readonly IChatService<PersonalChatRequest, PersonalChatDTO> _chatService = chatService;
    private readonly IMessageService _chatMessageService = chatMessageService;
    private readonly UserHelper _userHelper = userHelper;

    private static readonly List<string> _connectedUsers = [];

    public override async Task OnConnectedAsync()
    {
        var chats = await UpdateUserStatus(true);

        await Clients.Caller.ReceiveChatList(chats.Cast<BaseChatDTO>().ToList());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
        =>  await UpdateUserStatus(false);

    private async Task<List<PersonalChatDTO>> UpdateUserStatus(bool isOnline)
    {
        var user = await _userHelper.GetCurrentUserAsync(Context?.User);

        if (isOnline)
            _connectedUsers.Add(user.Id);
        else
            _connectedUsers.Remove(user.Id);

        var chat = await _chatService.GetChatByIdAsync(2);
        List<PersonalChatDTO> chats = await _chatService.GetChatListByUserAsync(user);

        chats.ForEach(c =>
        {
            c.Users.First(u => u.Id.Equals(user.Id)).IsOnline = isOnline;
            UserDTO anotherUser = c.Users.First(u => !u.Id.Equals(user.Id));

            if (_connectedUsers.Contains(anotherUser.Id))
            {
                anotherUser.IsOnline = true;
                Clients.User(anotherUser.Id).ReceiveChat(c);
            }
        });

        return chats;
    }


    public async Task CreateChatAsync(PersonalChatRequest chatRequest)
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
            await Clients.All.ReceiveRemovedChatId(chatId);
        else
            await Clients.Caller.ReceiveErrorMessage("Failed to delete this chat.");
    }

    public async Task SendMessageAsync(MessageRequest messageRequest)
    {
        MessageDTO messageDTO = await _chatMessageService.CreateMessageAsync(messageRequest, await _userHelper.GetCurrentUserAsync(Context?.User));

            messageDTO.Chat?.Users
            .ForEach(async u => await Clients.User(u.Id).ReceiveMessage(messageDTO));
    }
}
