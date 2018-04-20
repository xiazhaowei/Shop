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
    public class PartnerQueryService : BaseQueryService,IPartnerQueryService
    {
        public Partner Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Partner>(new { Id = id }, ConfigSettings.PartnerTable).SingleOrDefault();
            }
        }
        public Partner FindByUserId(Guid userId)
        {
            using (var connection = GetConnection())
            {
               var partners= connection.QueryList<Partner>(new { UserId = userId }, ConfigSettings.PartnerTable);
                if (partners.Any() && partners.Count() == 1)
                {
                    return partners.SingleOrDefault();
                }
                return null;
            }
        }

        public IEnumerable<Partner> Partners()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Partner>(null, ConfigSettings.PartnerTable);
            }
        }

        public IEnumerable<Partner> UserPartners(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Partner>(new { UserId=userId}, ConfigSettings.PartnerTable);
            }
        }
    }
}