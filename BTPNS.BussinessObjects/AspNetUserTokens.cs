using Microsoft.AspNetCore.Identity;

namespace BTPNS.BussinessObjects
{
    public partial class AspNetUserTokens : IdentityUserToken<string>
    {
        public virtual AspNetUsers User { get; set; }
    }
}