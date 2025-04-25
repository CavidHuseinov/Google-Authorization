
using Auth.Business.Helpers.Common;

namespace Auth.Business.Helpers.DTOs.UserDto
{
    public record TokenDto:BaseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
