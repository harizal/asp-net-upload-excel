using System;
using System.Collections.Generic;

namespace BTPNS.Core.Repositories
{
    public interface IStoredProcedureRepository
    {
        void Save(Dictionary<KeyValuePair<int, int>, object> datas, string tableName);
        Tuple<List<string>, List<List<string>>> Save(List<string> header, List<List<string>> datas, string tableName);
    }
}
