using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Wallets.BenevolenceTransfers
{
    [Serializable]
    public class BenevolenceTransferStatusChangedEvent : DomainEvent<Guid>
    {
        public decimal FinallyValue { get; private set; }
        public BenevolenceTransferStatus Status { get; private set; }

        public BenevolenceTransferStatusChangedEvent() { }
        public BenevolenceTransferStatusChangedEvent(BenevolenceTransferStatus status,decimal finallyValue)
        {
            Status = status;
            FinallyValue = finallyValue;
        }
    }
}
