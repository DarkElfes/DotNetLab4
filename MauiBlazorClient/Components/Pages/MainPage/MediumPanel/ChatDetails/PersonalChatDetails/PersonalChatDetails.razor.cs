using Microsoft.AspNetCore.Components;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Components.Pages.MainPage.MediumPanel.ChatDetails.PersonalChatDetails;

public partial class PersonalChatDetails
{
    [Parameter] public PersonalChatDTO? Chat { get; set; }
}
