using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Wallets.RechargeApplys
{
    public class ChangeRechargeStatusCommand : Command<Guid>
    {
        public Guid RechargeApplyId { get; private set; }
        public RechargeApplyStatus Status { get; private set; }
        public string Remark { get;private set; }

        public ChangeRechargeStatusCommand() { }
        public ChangeRechargeStatusCommand(Guid rechargeApplyId,
            RechargeApplyStatus status,string remark)
        {
            RechargeApplyId = rechargeApplyId;
            Status = status;
            Remark = remark;
        }
    }
}
