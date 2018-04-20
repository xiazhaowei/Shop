using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// 查询服务接口
    /// </summary>
    public interface IAnnouncementQueryService
    {
        Announcement Find(Guid id);
        IEnumerable<Announcement> ListPage();
    }
}