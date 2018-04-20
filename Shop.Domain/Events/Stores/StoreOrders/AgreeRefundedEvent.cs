using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders
{
    [Serializable]
    public class AgreeRefundedEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal CashRefundAmount { get; set; }
        public decimal ShopCashRefundAmount { get; set; }

        public AgreeRefundedEvent() { }
        public AgreeRefundedEvent(Guid walletId,decimal refundAmount,decimal cashRefundAmount,decimal shopCashRefundAmount)
        {
            WalletId = walletId;
            RefundAmount = refundAmount;
            CashRefundAmount = cashRefundAmount;
            ShopCashRefundAmount = shopCashRefundAmount;
        }
    }
}
