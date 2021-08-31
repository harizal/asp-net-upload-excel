using BTPNS.BussinessObjects;
using BTPNS.Contracts;
using BTPNS.Core.Repositories;
using BTPNS.DAL.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTPNS.BLL
{
    public class BLL : IBLL
    {
        private IUnitOfWork<BTPNSDbContext> _uow { get; set; }
        private IGenericRepository<AspNetRoles> _rolesRepo { get; set; }
        private IGenericRepository<UserPermission> _userPermissionRepo { get; set; }
        private IStoredProcedureRepository _storedProcedureRepository;
        public BLL(IUnitOfWork<BTPNSDbContext> uow, IStoredProcedureRepository storedProcedureRepository)
        {
            _uow = uow;
            _rolesRepo = _uow.GetGenericRepository<AspNetRoles>();
            _userPermissionRepo = _uow.GetGenericRepository<UserPermission>();
            _storedProcedureRepository = storedProcedureRepository;
        }

        public Task<AspNetRoles> GetUserRoleByNameAsync(string roleName)
        {
            try
            {
                var role = _rolesRepo.GetAsQueryable(x => x.Name == roleName).FirstOrDefault();
                return Task.FromResult(role);
            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurred while getting the user role");
            }
        }

        public List<string> GetPermissionForUserRole(string roleId)
        {
            try
            {
                var userPermission = new List<string>();
                userPermission = _userPermissionRepo.GetAsQueryable(x => x.RoleId == roleId).Select(x => x.Action).ToList();
                return userPermission;
            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurred while getting the user permission");
            }
        }

        public void Save(Dictionary<KeyValuePair<int, int>, object> datas, string tableName)
        {
            _storedProcedureRepository.Save(datas, tableName);
        }

        public Tuple<List<string>, List<List<string>>> Save(List<string> header, List<List<string>> datas, string tableName)
        {
           return _storedProcedureRepository.Save(header, datas, tableName);
        }
    }
}
