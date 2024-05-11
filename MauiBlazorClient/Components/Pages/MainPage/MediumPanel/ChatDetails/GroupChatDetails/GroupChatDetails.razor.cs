using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Components.Pages.MainPage.MediumPanel.ChatDetails.GroupChatDetails;

public partial class GroupChatDetails
{
    [Inject] public IGroupChatService GroupChatService { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Parameter] public GroupChatDTO? Chat { get; set; }

    [CascadingParameter(Name = "CurrentUser")]
    public UserDTO CurrentUser { get; set; } = null!;


    protected void AddUser()
        => DialogService.ShowForm<AddUserRequest>(
            "Add users in chat",
            async model => await GroupChatService.AddUserAsync((AddUserRequest)model!, Chat.Id)
            );
}
