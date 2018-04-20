using ENode.Eventing;
using Shop.Domain.Models.Stores;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class StoreOrderConfirmExpressedEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public Guid StoreOwnerWalletId { get;private set; }
        public decimal StoreGetAmount { get;private  set; }
        public IList<OrderGoodsInfo> OrderGoodses { get; private set; }

        public StoreOrderConfirmExpressedEvent() { }
        public StoreOrderConfirmExpressedEvent(
            Guid walletId,
            Guid storeOwnerWalletId,
            decimal storeGetAmount,
            IList<OrderGoodsInfo> orderGoodses)
        {
            WalletId = walletId;
            StoreOwnerWalletId = storeOwnerWalletId;
            StoreGetAmount = storeGetAmount;
            OrderGoodses = orderGoodses;
        }

    }
}
