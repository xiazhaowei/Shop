using ENode.Eventing;
using Shop.Common.Enums;
using System;

namespace Shop.Domain.Events.Wallets.RechargeApplys
{
    public class RechargeApplyStatusChangedEvent:DomainEvent<Guid>
    {
        public Guid RechargeApplyId { get; private set; }
        public RechargeApplyStatus Status { get; private set; }
        public string Remark { get; private set; }

        public RechargeApplyStatusChangedEvent() { }
        public RechargeApplyStatusChangedEvent(Guid rechargeApplyId,
            RechargeApplyStatus status,string remark)
        {
            RechargeApplyId = rechargeApplyId;
            Status = status;
            Remark = remark;
        }
    }
}
