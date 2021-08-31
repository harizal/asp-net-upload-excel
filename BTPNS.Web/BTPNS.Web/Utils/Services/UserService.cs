using BTPNS.BussinessObjects;
using BTPNS.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BTPNS.Web.Utils.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AspNetUsers> _userManager;
        public UserService(IHttpContextAccessor httpContextAccessor,
            UserManager<AspNetUsers> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public Task<AspNetUsers> User => _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

        Task<int> IUserService.UserId => Task.FromResult(User.Id);

    }
}