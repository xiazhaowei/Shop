using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Wallets.ShopCashTransfers
{
    [Serializable]
    public class ShopCashTransferStatusChangedEvent:DomainEvent<Guid>
    {
        public ShopCashTransferStatus Status { get; private set; }
        public decimal FinallyValue { get; private set; }

        public ShopCashTransferStatusChangedEvent() { }
        public ShopCashTransferStatusChangedEvent(ShopCashTransferStatus status,decimal finallyValue)
        {
            Status = status;
            FinallyValue = finallyValue;
        }
    }
}
