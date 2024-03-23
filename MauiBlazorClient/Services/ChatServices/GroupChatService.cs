using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Request.Chat;

namespace MauiBlazorClient.Services.ChatServices;

public class GroupChatService(
    IModalDialogService modalDialogService,
    IConfiguration configuration
    ) : BaseChatService<GroupChatRequest>(modalDialogService, configuration), IGroupChatService
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

    private void CreateGroupHubMethods()
    {

    }
}
