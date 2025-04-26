using Auth.Core.Entities.Identity;
using Auth.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Auth.WebAPI
{
    public static class WebAPIInstallerService
    {

        public static void WebAPIInstaller(IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Lockout.MaxFailedAccessAttempts = 4;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:SecurityKey"])
                    ),
                    ClockSkew = TimeSpan.Zero
                };

            });

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv7", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

        }
    }
}
