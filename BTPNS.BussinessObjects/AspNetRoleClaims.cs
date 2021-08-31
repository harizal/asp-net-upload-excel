using Microsoft.AspNetCore.Identity;

namespace BTPNS.BussinessObjects
{
    public partial class AspNetRoleClaims : IdentityRoleClaim<string>
    {
        public virtual AspNetRoles Role { get; set; }
    }
}