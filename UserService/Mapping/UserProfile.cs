using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserService.Models;
namespace UserService.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDTO, User>();
            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<User, PublicUserDto>();
            //CreateMap<User, object>()
            //    .ForMember(dest => dest, opt => opt.MapFrom(src => new
            //    {
            //        src.Id,
            //        src.Username,
            //        src.Email,
            //        src.Bio,
            //        src.ProfileImageUrl
            //    }));
        }
    }
}
