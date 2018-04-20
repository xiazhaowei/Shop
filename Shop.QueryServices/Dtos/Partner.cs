using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class Partner
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public PartnerLevel Level { get; set; }
        public decimal Persent { get; set; }
        public decimal CashPersent { get; set; }
        public int BalanceInterval { get; set; }
        public decimal LastBalancedAmount { get; set; }
        public decimal TotalBalancedAmount { get; set; }
        public DateTime BalancedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}
