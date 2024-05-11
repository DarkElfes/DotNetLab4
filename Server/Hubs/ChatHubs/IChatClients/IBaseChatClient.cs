using Shared.DTOs.Response.ChatsDTO;
using Shared.DTOs.Response;

namespace Server.Hubs.ChatHubs.IChatClients;

public interface IBaseChatClient
{
    Task ReceiveChatList(List<BaseChatDTO> chats);
    Task ReceiveChat(BaseChatDTO chat);
    Task ReceiveRemovedChatId(int chatId);


    Task ReceiveMessage(MessageDTO messageDTO);

    Task ReceiveErrorMessage(string errorMessage);
}
