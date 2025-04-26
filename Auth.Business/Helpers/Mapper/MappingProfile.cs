
using Auth.Business.Helpers.DTOs.UserDto;
using Auth.Core.Entities.Identity;
using AutoMapper;

namespace Auth.Business.Helpers.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();
            #endregion

            #region UserToken
            CreateMap<TokenDto, UserToken>().ReverseMap();
            #endregion
        }
    }
}
