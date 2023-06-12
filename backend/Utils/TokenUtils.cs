using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace SuperHeroApi.Utils
{
    public static class TokenUtils {

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp) {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeVal;
        }

        public static string GenerateRandomRefreshToken() {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
