using BTPNS.BussinessObjects;
using System.Threading.Tasks;

namespace BTPNS.Contracts
{
    public interface IUserService
    {
        Task<int> UserId { get; }
        Task<AspNetUsers> User { get; }
    }
}