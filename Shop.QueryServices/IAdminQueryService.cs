using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// 查询服务接口
    /// </summary>
    public interface IAdminQueryService
    {
        Admin Find(Guid id);
        Admin Find(string loginName);
        IEnumerable<Admin> Admins();
        IEnumerable<OperatRecord> OperatRecords();
    }
}