using Microsoft.AspNetCore.Identity;

namespace BTPNS.BussinessObjects
{
    public partial class AspNetUserLogins : IdentityUserLogin<string>
    {
        public string Id { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
