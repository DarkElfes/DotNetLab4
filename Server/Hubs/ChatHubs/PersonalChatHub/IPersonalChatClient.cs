using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Hubs.ChatHubs.PersonalChatHub;

public interface IPersonalChatClient
{
    Task ReceiveChats(List<BaseChatDTO> chats);
    Task ReceiveChat(BaseChatDTO chat);
    Task ReceiveRemovedChatId(int chatId);


    Task ReceiveMessage(MessageDTO messageDTO);
    Task ReceiveErrorMessage(string errorMessage);
}
