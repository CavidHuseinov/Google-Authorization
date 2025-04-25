
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities.Identity
{
    public class User:IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
