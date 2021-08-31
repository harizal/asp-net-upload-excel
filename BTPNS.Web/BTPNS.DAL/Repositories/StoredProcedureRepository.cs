using BTPNS.Core;
using BTPNS.Core.Repositories;
using BTPNS.DAL.EntityFramework.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BTPNS.DAL.Repositories
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly IConfiguration _configuration;
        private readonly BTPNSDbContext _context;
        public StoredProcedureRepository(BTPNSDbContext context,
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Save(Dictionary<KeyValuePair<int, int>, object> datas, string tableName)
        {
            var queryCreateTable = $"CREATE TABLE [dbo].[{tableName.RemoveSpecialCharacter()}] ( ";
            foreach (var item in datas)
            {
                if (item.Key.Key == 1)
                {
                    queryCreateTable += $"[{item.Value.ToString().RemoveSpecialCharacter()}] [NVARCHAR](MAX) NULL,";
                }
                else
                    break;
            }

            queryCreateTable = queryCreateTable.TrimEnd(',') + ")";

            var queryInsert = $"INSERT INTO [dbo].[{tableName.RemoveSpecialCharacter()}] ( ";
            var queryInsertWithDatas = $"(";
            foreach (var item in datas)
            {
                if (item.Key.Key == 1)
                {
                    queryInsert += $"[{item.Value.ToString().RemoveSpecialCharacter()}],";
                }
                else
                {
                    queryInsertWithDatas += $"'{item.Value}',";
                }
            }

            queryInsert = $"{queryInsert.TrimEnd(',')} )VALUES {queryInsertWithDatas.TrimEnd(',')} )";
        }

        public Tuple<List<string>, List<List<string>>> Save(List<string> header, List<List<string>> datas, string tableName)
        {
            var conString = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            using (var con = new SqlConnection(conString))
            {
                var query = GenerateScript(header, datas, tableName);
                SqlCommand sqlCommand = new SqlCommand(query, con);
                con.Open();
                sqlCommand.ExecuteNonQuery();
            }

            return new Tuple<List<string>, List<List<string>>>(header, datas);
        }

        private string GenerateScript(List<string> header, List<List<string>> datas, string tableName)
        {
            var queryCreateTable = $" IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName.RemoveSpecialCharacter()}') BEGIN DROP TABLE {tableName.RemoveSpecialCharacter()} END " + //this script for Drop existing Table
                                               $"" +
                                               $"CREATE TABLE [dbo].[{tableName.RemoveSpecialCharacter()}] ( ";
            var queryInsert = $"INSERT INTO [dbo].[{tableName.RemoveSpecialCharacter()}] ( ";
            foreach (var item in header)
            {
                queryCreateTable += $"[{item.RemoveSpecialCharacter()}] [NVARCHAR](MAX) NULL,";
                queryInsert += $"[{item.ToString().RemoveSpecialCharacter()}],";
            }
            queryCreateTable = queryCreateTable.TrimEnd(',') + ")   ";

            queryInsert = queryInsert.TrimEnd(',') + ") VALUES ";
            foreach (var item in datas)
            {
                var queryInsertWithDatas = $"(";
                foreach (var value in item)
                {
                    queryInsertWithDatas += $"'{value}',";
                }
                queryInsert += queryInsertWithDatas.TrimEnd(',') + "),";
            }

            return queryCreateTable + queryInsert.TrimEnd(',');
        }
    }
}
