
using Auth.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Auth.Business.SeedDatas
{
    public class RoleSeed
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }
    }
}
