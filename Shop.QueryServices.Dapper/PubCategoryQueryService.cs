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
    /// ��ѯ���� ʵ��
    /// </summary>
    [Component]
    public class PubCategoryQueryService : BaseQueryService,IPubCategoryQueryService
    {
        public PubCategory Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<PubCategory>(new { Id = id }, ConfigSettings.PubCategoryTable).SingleOrDefault();
            }
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PubCategory> RootCategorys()
        {
            
            using (var connection = GetConnection())
            {
                return connection.QueryList<PubCategory>(new { ParentId = Guid.Empty}, ConfigSettings.PubCategoryTable);
            }
        }

        public IEnumerable<PubCategory> GetChildren(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<PubCategory>(new { ParentId = id }, ConfigSettings.PubCategoryTable);
            }
        }
    }
}