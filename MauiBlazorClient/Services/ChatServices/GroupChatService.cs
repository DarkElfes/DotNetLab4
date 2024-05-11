using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Request.Chat;

namespace MauiBlazorClient.Services.ChatServices;

public class GroupChatService(
    IDialogService dialogService,
    IConfiguration configuration
    ) : BaseChatService<GroupChatRequest>(dialogService, configuration), IGroupChatService
{
    public override async Task ConnectAsync(string token)
    {
        if (!TryBuildHubConnection("/chatHub/group", token))
            return;

        CreateBaseHubMethods();
        CreateGroupHubMethods();

        await HubConnection!.StartAsync();
    }

    public async Task LeaveFromChatAsync(int chatId)
        => await HubConnection!.SendAsync("LeaveFromChatAsync", chatId);

    public async Task AddUserAsync(AddUserRequest addUserRequest, int chatId)
        => await HubConnection!.SendAsync("AddUserAsync", addUserRequest, chatId);

    private void CreateGroupHubMethods()
    {
        
    }
}
