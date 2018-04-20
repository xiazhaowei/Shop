using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class RechargeApply
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string Pic { get; set; }
        public string BankNumber { get; set; }
        public string BankOwner { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
        public RechargeApplyStatus Status { get; set; }
    }
}
