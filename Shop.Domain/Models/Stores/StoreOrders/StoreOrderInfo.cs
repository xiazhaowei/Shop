using System;

namespace Shop.Domain.Models.Stores.StoreOrders
{
    public class StoreOrderInfo
    {
        public Guid UserId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid StoreId { get; private set; }
        public string Region { get; private set; }
        public string Number { get; private set; }
        public string Remark { get;private set; }

        public StoreOrderInfo(
            Guid userId,
            Guid orderId,
            Guid storeId,
            string region,
            string number,
            string remark
            )
        {
            UserId = userId;
            OrderId = orderId;
            StoreId = storeId;
            Region = region;
            Number = number;
            Remark = remark;
        }

    }
}
