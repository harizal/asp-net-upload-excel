using BTPNS.BussinessObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTPNS.Contracts
{
    public interface IBLL
    {
        Task<AspNetRoles> GetUserRoleByNameAsync(string roleName);
        List<string> GetPermissionForUserRole(string roleId);
        void Save(Dictionary<KeyValuePair<int, int>, object> datas, string tableName);
        Tuple<List<string>, List<List<string>>> Save(List<string> header, List<List<string>> datas, string tableName);
    }
}
