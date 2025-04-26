
using Google.Apis.Auth;

namespace Auth.Business.Services.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
    }
}
