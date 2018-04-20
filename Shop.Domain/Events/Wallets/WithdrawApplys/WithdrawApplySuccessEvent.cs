using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Wallets.WithdrawApplys
{
    public class WithdrawApplySuccessEvent:DomainEvent<Guid>
    {
        public Guid WithdrawApplyId { get; private set; }
        public decimal FinalLockedCash { get; private set; }
        public decimal Amount { get; private set; }

        public WithdrawApplySuccessEvent() { }
        public WithdrawApplySuccessEvent(
            Guid withdrawApplyId,
            decimal amount,
            decimal finalLockedCash)
        {
            WithdrawApplyId = withdrawApplyId;
            Amount = amount;
            FinalLockedCash = finalLockedCash;
        }
    }
}
