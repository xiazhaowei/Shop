using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class ChangeWithdrawApplyStatusRequest
    {
        public Guid WithdrawApplyId { get; set; }
        public Guid WalletId { get; set; }
        public WithdrawApplyStatus Status { get; set; }
        public string Remark { get; set; }
    }

    
}