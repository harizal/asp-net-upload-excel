using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BTPNS.Core
{
    public static class PermissionHelper
    {
        public static bool GetPermission(string action)
        {
            var permissionAllowed = false;
            var httpContextAccessor = new HttpContextAccessor();
            var user = httpContextAccessor.HttpContext.User;
            if (user != null)
            {
                var claims = user.Claims?.ToList();
                var permissionsString = claims.Where(x => x.Type == "Permissions").FirstOrDefault();
                if (permissionsString != null)
                {
                    var permissions = permissionsString.Value.Split(";");
                    permissionAllowed = permissions.Any(x => x == action);
                }
            }
            return permissionAllowed;
        }
    }
}