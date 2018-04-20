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
    public class NotificationQueryService : BaseQueryService,INotificationQueryService
    {
        public Notification Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Notification>(new { Id = id }, ConfigSettings.NotificationTable).SingleOrDefault();
            }
        }

        public IEnumerable<Notification> Notifications()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Notification>(null, ConfigSettings.NotificationTable);
            }
        }

        public IEnumerable<Notification> UserNotifications(Guid userId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Notification>(new { UserId=userId}, ConfigSettings.NotificationTable);
            }
        }
    }
}