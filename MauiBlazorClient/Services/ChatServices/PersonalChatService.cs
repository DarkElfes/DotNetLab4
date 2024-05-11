using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Request.Chat;

namespace MauiBlazorClient.Services.ChatServices;

public class PersonalChatService(
    IDialogService dialogService,
    IConfiguration configuration
    ) : BaseChatService<PersonalChatRequest>(dialogService, configuration), IPersonalChatService
{
    public override async Task ConnectAsync(string token)
    {
        if (!TryBuildHubConnection("/chatHub/personal", token))
            return;

        CreateBaseHubMethods();
        
        await HubConnection!.StartAsync();
    }
}
