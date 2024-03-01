using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Hubs.ChatHubs.PersonalChatHub;
using Server.Models;
using Server.Services.IServices;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;
using System.Security.Claims;

namespace Server.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub(
    IChatService<PersonalChatRequest> personalChatService,
    IChatService<GroupChatRequest> groupChatService,
    IMessageService chatMessageService,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext db
    ) : Hub<IPersonalChatClient>
{
    private readonly IChatService<PersonalChatRequest> _personalChatService = personalChatService;
    private readonly IChatService<GroupChatRequest> _groupChatService = groupChatService;
    private readonly IMessageService _chatMessageService = chatMessageService;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _db = db;


    public override async Task OnConnectedAsync()
    {
        ApplicationUser? currentUser = await GetCurrentUser();

        List<BaseChatDTO> chatsDTO = _mapper.Map<List<BaseChatDTO>>(await _db.BaseChats
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .Where(c => c.Users.Any(u => u == currentUser))
            .ToListAsync());

        foreach (GroupChatDTO group in chatsDTO.Where(c => c is GroupChatDTO))
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);

        await Clients.Caller.ReceiveChats(chatsDTO);
    }


    public async Task CreateChatAsync(ChatRequest chatRequest)
    {
        try
        {
            BaseChatDTO chatDTO = chatRequest switch
            {
                PersonalChatRequest personalChatRequest when personalChatRequest is not null
                     => await _personalChatService.CreateChatAsync((PersonalChatRequest)chatRequest, await GetCurrentUser()),

                GroupChatRequest groupChatRequest when groupChatRequest is not null
                     => await _groupChatService.CreateChatAsync((GroupChatRequest)chatRequest, await GetCurrentUser()),

                _ => throw new NotImplementedException("Not realized chat type")
            };

            await Clients.Caller.ReceiveChat(chatDTO);
        }
        catch (Exception ex)
        {
            await SendErrorMessageAsync(ex.Message);
        }
    }
    public async Task RemoveChatAsync(int chatId)
    {
        try
        {
            _db.BaseChats.Remove(await _db.BaseChats.FirstOrDefaultAsync(c => c.Id == chatId));
            await Clients.Caller.ReceiveRemovedChatId(chatId);
        }
        catch (Exception ex)
        {
            await SendErrorMessageAsync(ex.Message);
        }
    }
    public async Task SendMessageAsync(MessageRequest messageRequest)
    {
        MessageDTO messageDTO = await _chatMessageService.CreateMessageAsync(messageRequest, await GetCurrentUser());

        foreach (UserDTO user in messageDTO!.Chat!.Users)
            await Clients.User(user.Id).ReceiveMessage(messageDTO);
    }

    public async Task AddUserAsync()
    {

    }
    public async Task RemoveUserAsync()
    {

    }

    private async Task<ApplicationUser> GetCurrentUser()
    {
        ClaimsPrincipal claimsPrincipal = Context?.User ??
            throw new ArgumentNullException("Failed to retrieve the current user.");

        if (await _userManager.GetUserAsync(claimsPrincipal) is ApplicationUser currentUser)
            return currentUser;

        throw new ArgumentException("Current user not found, try to relogin.");
    }
    private async Task SendErrorMessageAsync(string errorMessage)
        => await Clients.Caller.ReceiveErrorMessage(errorMessage);
}
