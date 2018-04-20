using ENode.Eventing;
using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.WithdrawApplys;
using System;

namespace Shop.Domain.Events.Wallets.WithdrawApplys
{
    public class WithdrawApplyCreatedEvent:DomainEvent<Guid>
    {
        public Guid WithdrawApplyId { get; private set; }
        public decimal FinalCash { get; private set; }
        public decimal FinalLockedCash { get; private set; }
        public WithdrawApplyInfo Info { get; private set; }

        public WithdrawApplyCreatedEvent() { }
        public WithdrawApplyCreatedEvent(
            Guid withdrawApplyId,
            decimal finalCash,
            decimal finalLockedCash,
            WithdrawApplyInfo info)
        {
            WithdrawApplyId = withdrawApplyId;
            FinalCash = finalCash;
            FinalLockedCash = finalLockedCash;
            Info = info;
        }
    }
}
