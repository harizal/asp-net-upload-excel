using BTPNS.BussinessObjects;
using BTPNS.Web.Utils.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BTPNS.Web.Utils.Handlers
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private UserManager<AspNetUsers> _userManager;

        public PermissionAuthorizationHandler(UserManager<AspNetUsers> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            bool permission = false;
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                return;
            }
            var claims = await _userManager.GetClaimsAsync(user);

            var permissions = claims.Where(x => x.Type == "Permissions").FirstOrDefault();
            if (permissions != null)
            {
                var permissionsList = permissions.Value.Split(";");
                permission = permissionsList.Any(x => x == requirement.Permission);
            }
            if (permission)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}