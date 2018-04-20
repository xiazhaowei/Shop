using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class WithdrawApply
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string BankOwner { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
        public WithdrawApplyStatus Status { get; set; }
    }
}
