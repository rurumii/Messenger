using AutoMapper;
using MessageService.Models;

namespace MessageService.Mapping
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<CreateChatDTO, Chat>();
            CreateMap<UpdateChatNameDTO, Chat>();
        }
    }
}
