using BTPNS.BussinessObjects;
using BTPNS.Core;
using BTPNS.DAL.EntityFramework.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTPNS.DAL.EntityFramework.Context
{
    public class BTPNSDbContext : IdentityDbContext<AspNetUsers, AspNetRoles, string>, IDbContext
    {
        public BTPNSDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserPermissionConfiguration());
        }
    }
}