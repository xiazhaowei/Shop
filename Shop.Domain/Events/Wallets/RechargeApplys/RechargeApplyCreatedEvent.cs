using ENode.Eventing;
using Shop.Common.Enums;
using Shop.Domain.Models.Wallets.RechargeApplys;
using System;

namespace Shop.Domain.Events.Wallets.RechargeApplys
{
    public class RechargeApplyCreatedEvent : DomainEvent<Guid>
    {
        public Guid RechargeApplyId { get; private set; }
        public RechargeApplyInfo Info { get; private set; }
        public RechargeApplyStatus Status { get; private set; }

        public RechargeApplyCreatedEvent() { }
        public RechargeApplyCreatedEvent(Guid rechargeApplyId, RechargeApplyInfo info, RechargeApplyStatus status)
        {
            RechargeApplyId = rechargeApplyId;
            Info = info;
            Status = status;
        }
    }
}
