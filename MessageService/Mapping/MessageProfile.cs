using AutoMapper;
using MessageService.Models;
namespace MessageService.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<SendMessageDTO, Message>();
        }
    }
}
