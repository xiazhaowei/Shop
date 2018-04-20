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
    public class AnnouncementQueryService : BaseQueryService, IAnnouncementQueryService
    {
        public Announcement Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Announcement>(new { Id = id }, ConfigSettings.AnnouncementTable).SingleOrDefault();
            }
        }

        public IEnumerable<Announcement> ListPage()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Announcement>(new {  }, ConfigSettings.AnnouncementTable);
            }
        }

        
    }
}