using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System.Collections.Generic;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// Q端查询服务 Dapper
    /// </summary>
    [Component]
    public class BenevolenceIndexQueryService: BaseQueryService,IBenevolenceIndexQueryService
    {
        public IEnumerable<BenevolenceIndex> ListPage()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<BenevolenceIndex>(new { }, ConfigSettings.BenevolenceIndexIncentivesTable);
            }
        }
        
    }
}
