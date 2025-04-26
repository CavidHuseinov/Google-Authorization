using Auth.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.DAL
{
    public static class DALInstallerService
    {
        public static void DALInstaller(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Deploy"));
            });
        }
    }
}
