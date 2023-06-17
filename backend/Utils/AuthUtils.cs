using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Utils {
    public static class AuthUtils {
        public static string getPrincipal(IHttpContextAccessor context) {
            return context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string getUserId(IHttpContextAccessor context) {
            return context.HttpContext?.User.FindFirstValue("userId");
        }
    }
}
