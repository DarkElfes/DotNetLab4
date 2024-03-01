using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Server.Models;
using Server.Models.Chats;
using Server.Models.Messages;
using Shared.DTOs;
using Shared.DTOs.Response;
using Shared.DTOs.Response.ChatsDTO;

namespace Server.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, UserDTO>()
            .ReverseMap();

        CreateMap<BaseChat, BaseChatDTO>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .Include<PersonalChat, PersonalChatDTO>()
            .Include<GroupChat, GroupChatDTO>()
            .ReverseMap();

        CreateMap<PersonalChat, PersonalChatDTO>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ReverseMap();


        CreateMap<GroupChat, GroupChatDTO>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ReverseMap();

        CreateMap<ChatMessage, MessageDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Chat, opt => opt.MapFrom(src => src.Chat))
            .ReverseMap();

    }
}
