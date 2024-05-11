using MauiBlazorClient.Services.Authentication;
using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs.Request.Chat;
using Shared.DTOs.Response.ChatsDTO;

namespace MauiBlazorClient.Services;

public record ChatManager(
    IPersonalChatService _personalChatService,
    IGroupChatService _groupChatService,
    AuthenticationStateProvider _authStateProvider
    ) : IChatService
{
    public event Action<List<BaseChatDTO>>? OnChatsChanged;
    public event Action<BaseChatDTO?>? OnCurrentChatChanged;

    public BaseChatDTO? CurrentChat { get; private set; }
    public List<BaseChatDTO> Chats { get; private set; } = null!;


    private IBaseChatService? _currentChatService;

    public async Task ConnectAsync()
    {
        var jwt = await ((AuthStateProvider)_authStateProvider).GetTokenAsync();
        string token = jwt.EncodedToken;

        await _personalChatService.ConnectAsync(token);
        await _groupChatService.ConnectAsync(token);

        _personalChatService.OnChatsChanged += UpdateChats;
        _groupChatService.OnChatsChanged += UpdateChats;
    }
    public async Task DisconnectAsync()
    {
        await _personalChatService.DisconnectAsync();
        await _groupChatService.DisconnectAsync();

        CurrentChat = null;
        Chats?.Clear();
    }

    public async Task SendMessageAsync(MessageRequest messageRequest)
    {
        messageRequest.ChatId = CurrentChat?.Id ?? 0;
        await _currentChatService?.SendMessageAsync(messageRequest);
    }

    private void UpdateChats(List<BaseChatDTO> chats)
    {
        Chats = [.. _personalChatService.Chats ?? [], .. _groupChatService.Chats ?? []];
        Chats = [.. Chats.OrderByDescending(c => c.Messages.Count != 0 ? c.Messages.Max(m => m.Timestamp) : DateTime.MinValue)];

        CurrentChat = Chats.FirstOrDefault(c => c.Id.Equals(CurrentChat?.Id));

        OnChatsChanged?.Invoke(Chats);
        OnCurrentChatChanged?.Invoke(CurrentChat);
    }

    public void OpenChat(BaseChatDTO chat)
    {
        CurrentChat = chat;
        _currentChatService = CurrentChat.GetType() == typeof(PersonalChatDTO) ? _personalChatService : _groupChatService;

        OnCurrentChatChanged?.Invoke(CurrentChat);
    }

    
}
