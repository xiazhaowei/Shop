using ENode.Eventing;
using Shop.Common;
using Shop.Domain.Models.Stores;
using Shop.Domain.Models.Stores.StoreOrders;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    /// <summary>
    /// 商家订单提交
    /// </summary>
    [Serializable]
    public class StoreOrderCreatedEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public Guid StoreOwnerWalletId { get; private set; }
        public StoreOrderInfo Info { get; private set; }
        public ExpressAddressInfo ExpressAddressInfo { get; private set; }
        public PayInfo PayInfo { get; private set; }
        public PayDetailInfo PayDetailInfo { get; private set; }
        public IList<OrderGoodsInfo> OrderGoodses { get; private set; }
        


        public StoreOrderCreatedEvent() { }
        public StoreOrderCreatedEvent(
            Guid walletId,
            Guid storeOwnerWalletId,
            StoreOrderInfo info,
            ExpressAddressInfo expressAddressInfo,
            PayInfo payInfo,
            PayDetailInfo payDetailInfo,
            IList<OrderGoodsInfo> orderGoodses)
        {
            WalletId = walletId;
            StoreOwnerWalletId = storeOwnerWalletId;
            Info = info;
            ExpressAddressInfo = expressAddressInfo;
            PayInfo = payInfo;
            PayDetailInfo = payDetailInfo;
            OrderGoodses = orderGoodses;
        }
    }
}
