using AutoMapper;
using MessageService.Models;
namespace MessageService.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<SendMessageDTO, Message>();
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.Ignore());
        }
    }
}
