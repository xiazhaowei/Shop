using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Wallet
{
    public class RechargeApplyLogsResponse:BaseApiResponse
    {
        public IList<RechargeApply> RechargeApplys { get; set; }
    }

    public class RechargeApply
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Pic { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string BankOwner { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }
}