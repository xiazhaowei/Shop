using Shop.Common.Enums;
using System;

namespace Shop.Domain.Models.Wallets.RechargeApplys
{
    public class RechargeApply
    {
        public Guid Id { get;private set; }
        public RechargeApplyInfo Info { get; private set; }

        public RechargeApplyStatus Status { get;  set; }

        public RechargeApply(Guid id, RechargeApplyInfo info, RechargeApplyStatus status)
        {
            Id = id;
            Info = info;
            Status = status;
        }
    }

   
}
