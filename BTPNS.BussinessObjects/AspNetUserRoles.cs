using Microsoft.AspNetCore.Identity;

namespace BTPNS.BussinessObjects
{
    public partial class AspNetUserRoles : IdentityUserRole<string>
    {
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
