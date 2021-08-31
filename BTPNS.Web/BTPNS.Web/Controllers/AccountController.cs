using BTPNS.BussinessObjects;
using BTPNS.Contracts;
using BTPNS.Core;
using BTPNS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BTPNS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly IStringLocalizer<AccountController> _stringLocalizer;
        private readonly IBLL _bll;
        private readonly IStringLocalizer<AppResources> _appLocalizer;

        public AccountController(IBLL bll,
            UserManager<AspNetUsers> userManager,
            SignInManager<AspNetUsers> signInManager,
            IStringLocalizer<AccountController> stringLocalizer,
            IStringLocalizer<AppResources> appLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _stringLocalizer = stringLocalizer;
            _appLocalizer = appLocalizer;
            _bll = bll;
        }        

        [HttpGet]
        public ActionResult Login()
        {
            var text = _stringLocalizer["Welcome Back"];
            var appName = _appLocalizer["AppName"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, false);
                var roles = await _userManager.GetRolesAsync(user);
                await AddUserPermissionClaims(user, roles.FirstOrDefault());
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Username or password is incorrect");
            }
            return View(model);
        }

        private async Task AddUserPermissionClaims(AspNetUsers user, string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                var userRole = await _bll.GetUserRoleByNameAsync(role);
                if (userRole != null)
                {
                    var permissions = _bll.GetPermissionForUserRole(userRole.Id);
                    var permissionsString = "";
                    foreach (var perm in permissions)
                    {
                        permissionsString = string.Concat(permissionsString, $"{perm}");
                        if (perm != permissions.Last())
                        {
                            permissionsString = string.Concat(permissionsString, ";");
                        }
                    }
                    await _userManager.AddClaimAsync(user, new Claim("Permissions", permissionsString));
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}