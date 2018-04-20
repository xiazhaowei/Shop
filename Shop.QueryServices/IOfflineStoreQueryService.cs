using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// 店铺查询接口
    /// </summary>
    public interface  IOfflineStoreQueryService
    {
        OfflineStore Info(Guid id);

        IEnumerable<OfflineStore> StoreList();
        IEnumerable<OfflineStore> UserStoreList(Guid userId);
        OfflineStore InfoByUserId(Guid userId);

        decimal TodaySale();
    }
}
