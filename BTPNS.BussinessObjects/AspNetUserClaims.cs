using Microsoft.AspNetCore.Identity;

namespace BTPNS.BussinessObjects
{
    public partial class AspNetUserClaims : IdentityUserClaim<string>
    {
        public virtual AspNetUsers User { get; set; }
    }
}
