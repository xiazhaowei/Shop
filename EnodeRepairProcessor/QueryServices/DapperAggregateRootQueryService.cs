using ECommon.Components;
using ECommon.Dapper;
using EnodeRepairProcessor.QueryServices.Dtos;
using Shop.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace EnodeRepairProcessor.QueryServices
{
    /// <summary>
    /// 查询服务 实现
    /// </summary>
    public class DapperAggregateRootQueryService : IAggregateRootQueryService
    {

        public IEnumerable<AggregateRootDto> AggregateRoots()
        {
            using (var connection = GetEnodeConnection())
            {
                return connection.QueryList<AggregateRootDto>(null, "PublishedVersion");
            }
        }


        public EventStreamDto EventStreamVersion(AggregateRootDto publishedVersion)
        {
            using (var connection = GetEnodeConnection())
            {
                var sql = @"select top 1 
                                AggregateRootId,
                                AggregateRootTypeName,
                                Version 
                                from EventStream 
                                where AggregateRootId=@AggregateRootId and AggregateRootTypeName=@AggregateRootTypeName 
                                order by Version desc";
                return connection.QueryFirstOrDefault<EventStreamDto>(sql, publishedVersion);
            }
        }



        protected virtual IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ConnectionString);
        }

        protected virtual IDbConnection GetEnodeConnection()
        {
            return new SqlConnection(ConfigSettings.ENodeConnectionString);
        }
    }
}