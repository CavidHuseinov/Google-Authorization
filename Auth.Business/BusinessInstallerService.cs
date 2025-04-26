using Auth.Business.Helpers.Mapper;
using Auth.Business.SeedDatas;
using Auth.Business.Services.Implementations;
using Auth.Business.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace Auth.Business
{
    public static class BusinessInstallerService
    {
        public static void BusinessInstaller(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());


            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            #endregion


        }

        public static async Task RoleSeedAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await RoleSeed.SeedRoleAsync(roleManager);
        }

    }
}
