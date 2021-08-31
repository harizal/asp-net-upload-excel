
using BTPNS.BussinessObjects;
using BTPNS.Contracts;
using BTPNS.Core;
using BTPNS.Core.Constans;
using BTPNS.Core.Repositories;
using BTPNS.DAL;
using BTPNS.DAL.EntityFramework.Context;
using BTPNS.DAL.Repositories;
using BTPNS.Web.Utils.Handlers;
using BTPNS.Web.Utils.Localizations;
using BTPNS.Web.Utils.Requirements;
using BTPNS.Web.Utils.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BTPNS.Web.Utils.Extentions
{
    public static class StartupExtensions
    {
        public static void InitializeDatabase(this IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(StartupExtensions));
            try
            {

                var userManager = serviceProvider.GetRequiredService<UserManager<AspNetUsers>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<AspNetRoles>>();
                var _uow = serviceProvider.GetRequiredService<IUnitOfWork<BTPNSDbContext>>();
                var _userPermissionRepo = _uow.GetGenericRepository<UserPermission>();

                Task<IdentityResult> userResult;
                Task<IdentityResult> userRole;
                var roleSuperAdmin = roleManager.FindByNameAsync(RoleConstants.SuperAdmin);
                roleSuperAdmin.Wait();
                if (roleSuperAdmin.Result == null)
                {
                    roleManager.CreateAsync(new AspNetRoles()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = RoleConstants.SuperAdmin,
                        NormalizedName = RoleConstants.SuperAdmin
                    }).Wait();
                }
                Task<AspNetUsers> voxSuperAdminExists = userManager.FindByEmailAsync("btpnSyariahAdmin@email.com");
                voxSuperAdminExists.Wait();
                if (voxSuperAdminExists.Result == null)
                {
                    var user = new AspNetUsers();
                    user.Email = "btpnSyariahAdmin@email.com";
                    user.UserName = "btpnSyariahAdmin@email.com";
                    user.EmailConfirmed = true;
                    user.PasswordHash = null;
                    user.NormalizedUserName = "btpnSyariahAdmin";

                    userResult = userManager.CreateAsync(user, "_Admin01");
                    userResult.Wait();
                    userRole = userManager.AddToRoleAsync(user, RoleConstants.SuperAdmin);
                    userRole.Wait();
                    if (roleSuperAdmin.Result == null)
                        roleSuperAdmin = roleManager.FindByNameAsync(RoleConstants.SuperAdmin);
                    roleSuperAdmin.Wait();
                    UserPermission permission = new UserPermission()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Action = PermissionConstants.ViewSuperAdminMode,
                        RoleId = roleSuperAdmin.Result.Id
                    };
                    _userPermissionRepo.Insert(permission);
                    _uow.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed Insert Admin");
            }
        }
        public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BTPNSDbContext>(options =>
              options.UseSqlServer(
                  configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AspNetUsers, AspNetRoles>(options => options.SignIn.RequireConfirmedAccount = false)
                 .AddDefaultUI()
                 .AddEntityFrameworkStores<BTPNSDbContext>()
                 .AddDefaultTokenProviders();
        }
        public static void AddProjectAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                foreach (var permission in typeof(PermissionConstants).GetFields())
                {
                    options.AddPolicy(permission.Name,
                    policy => policy.Requirements.Add(new PermissionRequirement(permission.Name)));
                }
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/accessDenied";
            });
        }
        public static void AddProjectLoclization(this IServiceCollection services)
        {
            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
        public static void AddProjectService(this IServiceCollection services)
        {
            services.AddTransient<IBLL, BTPNS.BLL.BLL>();
            services.AddTransient<IUnitOfWork<BTPNSDbContext>, SqlUnitOfWork<BTPNSDbContext>>();
            services.AddScoped(typeof(ICoreLocalizer<>), typeof(CoreLocalizer<>));
            services.AddTransient<IDbContext, BTPNSDbContext>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileManagement, FileManagement>();
            services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
        }
    }
}