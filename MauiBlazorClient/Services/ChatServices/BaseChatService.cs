using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services.ChatServices;

public abstract class BaseChatService<T>(
    IDialogService dialogService,
    IConfiguration configuration) where T : ChatRequest
{
    protected readonly IDialogService _dialogService = dialogService;
    protected readonly string _serverUrl = configuration.GetValue<string>("ServerApiUrl")
        ?? throw new ArgumentNullException("Not found server url");

    protected HubConnection HubConnection;

    public List<BaseChatDTO> Chats { get; set; } = null!;
    public event Action<List<BaseChatDTO>>? OnChatsChanged;


    public abstract Task ConnectAsync(string token);
    public async Task DisconnectAsync()
    {
        if (HubConnection is not null)
            await HubConnection.StopAsync();

        Chats?.Clear();
    }

    public async Task CreateChatAsync(T chatRequest)
        => await HubConnection!.SendAsync("CreateChatAsync", chatRequest);
    public async Task RemoveChatAsync(int chatId)
        => await HubConnection!.SendAsync("RemoveChatAsync", chatId);

    public async Task SendMessageAsync(MessageRequest messageRequest)
        => await HubConnection!.SendAsync("SendMessageAsync", messageRequest);


    protected bool TryBuildHubConnection(string pathToHub, string token)
    {
        try
        {
            HubConnection = new HubConnectionBuilder()
             .WithUrl(_serverUrl + pathToHub, options =>
                 options.AccessTokenProvider = () => Task.FromResult(token)!)
             .WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(1)])
             .AddNewtonsoftJsonProtocol(options =>
             {
                 options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.All;
                 options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             })
             .Build();

            return true;
        }
        catch
        {
            ShowErrorMessage("Failed to create chat hub");
            return false;
        }
    }
    protected void CreateBaseHubMethods()
    {
        HubConnection?.On<List<BaseChatDTO>?>("ReceiveChatList", chats
            => UpdateChats(() => Chats = chats ?? []));

        HubConnection?.On<BaseChatDTO>("ReceiveChat", chat
            => UpdateChats(() =>
            {
                if (Chats.FirstOrDefault(c => c.Id.Equals(chat.Id)) is BaseChatDTO oldChat)
                    Chats[Chats.IndexOf(oldChat)] = chat;
                else
                    Chats.Add(chat);
            }));

        HubConnection?.On<MessageDTO>("ReceiveMessage", message
            => UpdateChats(() => Chats.FirstOrDefault(c => c.Id == message?.Chat?.Id)?.Messages.Add(message)));

        HubConnection?.On<int>("ReceiveRemovedChatId", removedChatId
            => UpdateChats(() => Chats.Remove(Chats.FirstOrDefault(c => c.Id == removedChatId)!)));

        HubConnection?.On<string>("ReceiveErrorMessage", ShowErrorMessage);
    }

    protected void UpdateChats(Action action)
    {
        action.Invoke();
        UpdateUI(() => OnChatsChanged?.Invoke(Chats));
    }
    protected void UpdateUI(Action action)
        => MainThread.BeginInvokeOnMainThread(action);

    protected void ShowErrorMessage(string errorMessage)
        => UpdateUI(() => _dialogService.ShowNotification("Error", errorMessage));
}