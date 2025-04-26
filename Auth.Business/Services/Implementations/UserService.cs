
using Auth.Business.Helpers.DTOs.UserDto;
using Auth.Business.Services.Interfaces;
using Auth.Core.Entities.Identity;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IGoogleAuthService _googleAuth;

        public UserService(UserManager<User> userManager, IMapper mapper, IConfiguration config, IHttpContextAccessor httpContextAccesor, IGoogleAuthService auth)
        {
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
            _httpContextAccesor = httpContextAccesor;
            _googleAuth = auth;
        }
        private string GenerateAccessToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task Register(RegisterDto register)
        {
            if (await _userManager.FindByEmailAsync(register.Email) != null)
                throw new InvalidOperationException("Bu email artiq movcuddur");
            if (await _userManager.FindByNameAsync(register.Username) != null)
                throw new InvalidOperationException("Bu username artiq movcuddur");
            var mapRegister = _mapper.Map<User>(register);
            var createRegister = await _userManager.CreateAsync(mapRegister,register.Password);
            if (!createRegister.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var error in createRegister.Errors)
                {
                    sb.AppendLine(error.Description);
                }
                throw new InvalidOperationException(sb.ToString());
            }
            await _userManager.UpdateAsync(mapRegister);
        }
        public async Task<TokenDto> Login(LoginDto login)
        {
            User? user = await _userManager.FindByEmailAsync(login.UsernameOrEmail) ??
                        await _userManager.FindByNameAsync(login.UsernameOrEmail);
            if (user == null) throw new InvalidOperationException("Istifadeci adi ve ya sifre yanlisdir");
            var checkPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!checkPassword) throw new InvalidOperationException("Istifadeci adi ve ya sifre yanlisdir");
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = GenerateAccessToken(user, roles);
            var refreshToken = GenerateRefreshToken();
            UserToken token = new UserToken
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };

            _httpContextAccesor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<User> GoogleLoginAsync(string idToken)
        {
            var payload = await _googleAuth.ValidateGoogleTokenAsync(idToken);

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true,
                    Name = payload.GivenName,
                    Surname = payload.FamilyName
                };

                var createResult = await _userManager.CreateAsync(user, "Login@2033Password#");
                if (!createResult.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in createResult.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }
                    throw new InvalidOperationException(sb.ToString());
                }
            }

            return user;
        }



    }
}
