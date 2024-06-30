using System.Security.Cryptography;

namespace Buisness_Logic_Layer.AuthHelpers
{
    public class RefreshTokenService
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
