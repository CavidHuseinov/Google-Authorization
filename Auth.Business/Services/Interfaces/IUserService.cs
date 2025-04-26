
using Auth.Business.Helpers.DTOs.UserDto;
using Auth.Core.Entities.Identity;

namespace Auth.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterDto register);
        Task<TokenDto> Login(LoginDto login);
        Task<User> GoogleLoginAsync(string idToken);
    }
}
