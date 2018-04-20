using Shop.Common.Enums;
using System;

namespace Shop.Domain.Models.Wallets.WithdrawApplys
{
    public class WithdrawApply
    {
        public Guid Id { get;private set; }
        public WithdrawApplyInfo Info { get; private set; }

        public WithdrawApplyStatus Status { get;  set; }

        public WithdrawApply(Guid id,WithdrawApplyInfo info,WithdrawApplyStatus status)
        {
            Id = id;
            Info = info;
            Status = status;
        }
    }

   
}
