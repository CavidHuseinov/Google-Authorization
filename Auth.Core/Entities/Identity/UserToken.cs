
using Auth.Core.Entities.Common;

namespace Auth.Core.Entities.Identity
{
    public class UserToken:BaseEntity
    {
        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiration {  get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
    }
}
