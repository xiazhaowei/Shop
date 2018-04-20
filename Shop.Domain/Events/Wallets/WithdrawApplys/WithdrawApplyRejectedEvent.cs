using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Wallets.WithdrawApplys
{
    public class WithdrawApplyRejectedEvent:DomainEvent<Guid>
    {
        public Guid WithdrawApplyId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal FinalLockedCash { get; private set; }
        public string Remark { get; private set; }

        public WithdrawApplyRejectedEvent() { }
        public WithdrawApplyRejectedEvent(
            Guid withdrawApplyId,
            decimal amount,
            decimal finalLockedCash,
            string remark)
        {
            WithdrawApplyId = withdrawApplyId;
            Amount = amount;
            FinalLockedCash = finalLockedCash;
            Remark = remark;
        }
    }
}
