using Auth.Business;
using Auth.DAL;
using Bookifa.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace Auth.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region InstallerService
            builder.Services.DALInstaller(builder.Configuration);
            BusinessInstallerService.BusinessInstaller(builder.Services);
            WebAPIInstallerService.WebAPIInstaller(builder.Services,builder.Configuration);
            #endregion

            var app = builder.Build();
            #region SeedDataForRole
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await BusinessInstallerService.RoleSeedAsync(services);
            }
            #endregion

            app.UseCors("AllowAll");
            app.UseMiddleware<GlobalHandleException>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            app.Run();
        }
    }
}
