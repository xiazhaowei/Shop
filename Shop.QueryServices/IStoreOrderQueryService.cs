using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IStoreOrderQueryService
    {
        StoreOrderDetails FindOrder(Guid orderId);
        StoreOrder Find(Guid id);

        IEnumerable<StoreOrderWithInfo> StoreOrderList();

        IEnumerable<StoreOrder> StoreStoreOrders(Guid storeId);

        IEnumerable<StoreOrder> UserStoreOrders(Guid userId);
        IEnumerable<StoreOrderAlis> UserStoreOrderAlises(Guid userId);

        IEnumerable<StoreOrderAlis> StoreOrders();

        IEnumerable<StoreOrderDetails> UserStoreOrderDetails(Guid userId);
        IEnumerable<StoreOrderDetails> UserStoreOrderDetails(Guid userId, StoreOrderStatus status);

        IEnumerable<StoreOrderDetails> StoreStoreOrderDetails(Guid storeId);
        IEnumerable<StoreOrderDetails> StoreStoreOrderDetails(Guid storeId,StoreOrderStatus status);
    }
}
