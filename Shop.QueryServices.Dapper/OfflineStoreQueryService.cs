using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    [Component]
    public class OfflineStoreQueryService: BaseQueryService,IOfflineStoreQueryService
    {
        public OfflineStore Info(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<OfflineStore>(new { Id = id }, ConfigSettings.OfflineStoreTable).SingleOrDefault();
            }
        }
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OfflineStore InfoByUserId(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<OfflineStore>(new { UserId = userId }, ConfigSettings.OfflineStoreTable).SingleOrDefault();
            }
        }

        

        /// <summary>
        /// 今日所有商家销售额
        /// </summary>
        /// <returns></returns>
        public decimal TodaySale()
        {
            using (var connection = GetConnection())
            {
                var stores= connection.QueryList<OfflineStore>(new { }, ConfigSettings.OfflineStoreTable);
                return stores.Sum(x => x.TodaySale);
            }
        }

        public IEnumerable<OfflineStore> StoreList()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<OfflineStore>(new { }, ConfigSettings.OfflineStoreTable);
            }
        }

        public IEnumerable<OfflineStore> UserStoreList(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<OfflineStore>(new {UserId=userId }, ConfigSettings.OfflineStoreTable);
            }
        }
    }
}
