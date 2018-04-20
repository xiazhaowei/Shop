using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Wallet
{
    public class WithdrawApplyLogsResponse:BaseApiResponse
    {
        public IList<WithdrawApply> WithdrawApplys { get; set; }
    }

    public class WithdrawApply
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string BankOwner { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }
}