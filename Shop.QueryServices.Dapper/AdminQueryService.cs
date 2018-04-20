using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// 查询服务 实现
    /// </summary>
    [Component]
    public class AdminQueryService : BaseQueryService,IAdminQueryService
    {
        public Admin Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Admin>(new { Id = id }, ConfigSettings.AdminTable).SingleOrDefault();
            }
        }

        public Admin Find(string loginName)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Admin>(new { LoginName = loginName }, ConfigSettings.AdminTable).SingleOrDefault();
            }
        }

        public IEnumerable<Admin> Admins()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Admin>(null, ConfigSettings.AdminTable);
            }
        }

        public IEnumerable<OperatRecord> OperatRecords()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<OperatRecord>(null, ConfigSettings.AdminOperatRecordTable);
            }
        }
    }
}