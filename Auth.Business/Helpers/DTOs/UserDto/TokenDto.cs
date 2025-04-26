
namespace Auth.Business.Helpers.DTOs.UserDto
{
    public record TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
