using Dapper;
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
    public class StoreQueryService: BaseQueryService,IStoreQueryService
    {
        public Store Info(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Store>(new { Id = id }, ConfigSettings.StoreTable).SingleOrDefault();
            }
        }
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Store InfoByUserId(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Store>(new { UserId = userId }, ConfigSettings.StoreTable).SingleOrDefault();
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
                var stores= connection.QueryList<Store>(new { }, ConfigSettings.StoreTable);
                return stores.Sum(x => x.TodaySale);
            }
        }

        public IEnumerable<Store> StoreList()
        {
            var sql = string.Format(@"select a.*,b.Mobile 
            from {0} as a inner join {1} as b on a.UserId=b.Id", ConfigSettings.StoreTable, ConfigSettings.UserTable);

            using (var connection = GetConnection())
            {
                return connection.Query<Store>(sql);
            }
        }
    }
}
