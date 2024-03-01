using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices.PersonalChatService;

public class PersonalChatService(
    IConfiguration configuration,
    IModalDialogService modalDialogService
    ) : IPersonalChatService
{
    private readonly IModalDialogService _modalDialogService = modalDialogService;
    private readonly string _serverUrl = configuration.GetValue<string>("ServerApiUrl") ??
          throw new ArgumentNullException("Not found server url");

    private HubConnection? HubConnection { get; set; }

    public List<BaseChatDTO> Chats { get; set; } = null!;
    public event Action<List<BaseChatDTO>>? OnChatsChanged;


    public async Task ConnectAsync(string token)
    {
        HubConnection = new HubConnectionBuilder()
           .WithUrl(_serverUrl + "/chatHub/personal", options =>
               options.AccessTokenProvider = () => Task.FromResult(token)!)
           .WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(1)])
           .AddNewtonsoftJsonProtocol(options =>
           {
               options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.All;
               options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
           })
           .Build();

        CreateHubMethods();

        await HubConnection.StartAsync();
    }
    public async Task DisconnectAsync()
    {
        if (HubConnection is not null)
            await HubConnection.StopAsync();
        Chats?.Clear();
    }

    public async Task CreateChatAsync(PersonalChatRequest chatRequest)
        => await HubConnection!.SendAsync("CreateChatAsync", chatRequest);
    public async Task RemoveChatAsync(int chatId)
        => await HubConnection!.SendAsync("RemoveChatAsync", chatId);
    
    public async Task SendMessageAsync(MessageRequest messageRequest)
        => await HubConnection!.SendAsync("SendMessageAsync", messageRequest);



    private void CreateHubMethods()
    {
        HubConnection?.On<List<BaseChatDTO>?>("ReceiveChats", chats
            => UpdateChats(() => Chats = chats ?? []));

        HubConnection?.On<PersonalChatDTO>("ReceiveChat", chat
            => UpdateChats(() => Chats.Add(chat)));

        HubConnection?.On<MessageDTO>("ReceiveMessage", message
            => UpdateChats(() => Chats.FirstOrDefault(c => c.Id == message?.Chat?.Id)?.Messages.Add(message)));

        HubConnection?.On<int>("ReceiveRemovedChatId", removedChatId
            => UpdateChats(() => Chats.Remove(Chats.FirstOrDefault(c => c.Id == removedChatId)!)));

        HubConnection?.On<string>("ReceiveErrorMessage", erroMessage
            => UpdateUI(() => _modalDialogService.Show("Error", erroMessage)));
    }

    private void UpdateChats(Action action)
    {
        action.Invoke();
        UpdateUI(() => OnChatsChanged?.Invoke(Chats));
    }
    private void UpdateUI(Action action)
        => MainThread.BeginInvokeOnMainThread(action);
}
