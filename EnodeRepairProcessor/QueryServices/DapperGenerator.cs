using Dapper;
using ECommon.Components;
using EnodeRepairProcessor.QueryServices.Dtos;
using Shop.Common;
using System.Data.SqlClient;

namespace EnodeRepairProcessor.QueryServices
{
    public class DapperGenerator: IGenerator
    {
        public int DelEventStream(EventStreamDto eventStream)
        {
            var sql = @"delete from EventStream where AggregateRootId=@AggregateRootId and Version>@Version";

            using (var connection = GetConnection())
            {
                return connection.Execute(sql,eventStream);
            }
        }

        public int UpdatePublishedVersion(AggregateRootDto publishedVersion)
        {
            var sql = @"update PublishedVersion set Version=@Version where AggregateRootId=@AggregateRootId";
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, publishedVersion);
            }
        }

        protected  SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigSettings.ENodeConnectionString);
        }
    }
}
