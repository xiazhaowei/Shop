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
    public class CategoryQueryService : BaseQueryService,ICategoryQueryService
    {
        public Category Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Category>(new { Id = id }, ConfigSettings.CategoryTable).SingleOrDefault();
            }
        }
        /// <summary>
        /// 获取跟分类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> RootCategorys()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Category>(new { ParentId = Guid.Empty}, ConfigSettings.CategoryTable);
            }
        }

        public IEnumerable<Category> GetChildren(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Category>(new { ParentId = id }, ConfigSettings.CategoryTable);
            }
        }
    }
}