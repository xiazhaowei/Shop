using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets.RechargeApplys
{
    public class RechargeApplySuccessEvent:DomainEvent<Guid>
    {
        public decimal Amount { get; private set; }

        public RechargeApplySuccessEvent() { }
        public RechargeApplySuccessEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
