using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface IAnnouncementQueryService
    {
        Announcement Find(Guid id);
        IEnumerable<Announcement> ListPage();
    }
}