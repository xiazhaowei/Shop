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
    public class SectionQueryService : BaseQueryService, ISectionQueryService
    {
        public IEnumerable<SectionInfo> FindAll()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<SectionInfo>(null, ConfigSettings.SectionTable);
            }
        }
        public IEnumerable<SectionAndStatistic> FindAllInculdeStatistic()
        {
            var sql = string.Format(@"select Id
            ,Name
            ,[Description]
            ,(select count(0) from {0} where {0}.SectionId={1}.Id) as StoreCount 
            from {1}", ConfigSettings.StoreTable, ConfigSettings.SectionTable);

            using (var connection = GetConnection())
            {
                return connection.Query<SectionAndStatistic>(sql);
            }
        }
        public dynamic FindDynamic(Guid id, string option)
        {
            var columns = GetColumns(option);

            using (var connection = GetConnection())
            {
                return connection.QueryList(new { Id = id }, ConfigSettings.SectionTable, columns).SingleOrDefault();
            }

        }
        public SectionAndStatistic FindInculdeStatisticById(Guid id)
        {
            var sql = string.Format(@"select Id
            ,Name
            ,[Description]
            ,(select count(0) from {0} where {0}.SectionId={1}.Id) as PostCount 
            from {1} where {1}.Id=@Id", ConfigSettings.StoreTable, ConfigSettings.SectionTable);

            using (var connection = GetConnection())
            {
                return connection.Query<SectionAndStatistic>(sql, new { Id = id }).FirstOrDefault();
            }
        }

        private string GetColumns(string option)
        {
            string columns;
            switch (option)
            {
                case "simple":
                    columns = "Id,Name,Description";
                    break;
                case "detail":
                    columns = "Id,Name,Description,StoreCount";
                    break;
                default: throw new Exception("Invalid find option:" + option);
            }
            return columns;
        }
    }
}
