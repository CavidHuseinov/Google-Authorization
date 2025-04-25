
using Auth.Business.Helpers.DTOs.UserDto;

namespace Auth.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterDto register);
        Task<TokenDto> Login(LoginDto login);
    }
}
