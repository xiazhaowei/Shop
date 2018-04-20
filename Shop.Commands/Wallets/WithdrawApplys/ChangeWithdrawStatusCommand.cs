using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Wallets.WithdrawApplys
{
    public class ChangeWithdrawStatusCommand:Command<Guid>
    {
        public Guid WithdrawApplyId { get; private set; }
        public WithdrawApplyStatus Status { get; private set; }
        public string Remark { get;private set; }

        public ChangeWithdrawStatusCommand() { }
        public ChangeWithdrawStatusCommand(Guid withdrawApplyId,
            WithdrawApplyStatus status,string remark)
        {
            WithdrawApplyId = withdrawApplyId;
            Status = status;
            Remark = remark;
        }
    }
}
