using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class ChangeRechargeApplyStatusRequest
    {
        public Guid RechargeApplyId { get; set; }
        public Guid WalletId { get; set; }
        public RechargeApplyStatus Status { get; set; }
        public string Remark { get; set; }
    }

    
}