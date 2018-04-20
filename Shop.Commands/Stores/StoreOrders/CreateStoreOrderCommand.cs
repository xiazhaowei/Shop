using ENode.Commanding;
using Shop.Common;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Stores.StoreOrders
{
    /// <summary>
    /// 创建商家订单   由用户订单拆分过来的订单  和用户订单相同订单号
    /// </summary>
    public class CreateStoreOrderCommand:Command<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid StoreId { get; private set; }
        public Guid OrderId { get; private set; }
        public string Number { get; private set; }
        public string Remark { get; private set; }
        public ExpressAddressInfo ExpressAddressInfo { get; private set; }
        public PayInfo PayInfo { get; private set; }
        public PayDetailInfo PayDetailInfo { get; private set; }
        public IList<OrderGoods> OrderGoodses { get; private set; }

        public CreateStoreOrderCommand() { }
        public CreateStoreOrderCommand(
            Guid id,
            Guid userId,
            Guid storeId,
            Guid orderId,
            string number,
            string remark,
            ExpressAddressInfo expressAddressInfo,
            PayInfo payInfo,
            PayDetailInfo payDetailInfo,
            IList<OrderGoods> orderGoodses):base(id)
        {
            UserId = userId;
            StoreId = storeId;
            OrderId = orderId;
            Number = number;
            Remark = remark;
            ExpressAddressInfo = expressAddressInfo;
            PayInfo = payInfo;
            PayDetailInfo = payDetailInfo;
            OrderGoodses = orderGoodses;
        }
    }

    
}
