namespace BTPNS.BussinessObjects
{
    public partial class UserPermission
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Action { get; set; }

        public virtual AspNetRoles Role { get; set; }
    }
}