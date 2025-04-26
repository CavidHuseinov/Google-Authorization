
using Auth.Business.Services.Interfaces;
using Google.Apis.Auth;

namespace Auth.Business.Services.Implementations
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            return payload;
        }
    }
}
