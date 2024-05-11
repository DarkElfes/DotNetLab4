using Microsoft.AspNetCore.Components;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Components.Pages.MainPage.MediumPanel.ChatDetails;

public partial class ChatDetails
{
    [CascadingParameter(Name = "CurrentChat")]
    public BaseChatDTO? CurrentChat { get; set; }
}
