using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Partners
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Partner> Partners { get; set; }
    }

    public class Partner
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Level { get; set; }
        public decimal Persent { get; set; }
        public decimal CashPersent { get; set; }
        public int BalanceInterval { get; set; }
        public decimal LastBalancedAmount { get; set; }
        public decimal TotalBalancedAmount { get; set; }
        public string BalancedDate { get; set; }
        public string NextBalancedDate { get; set; }
        public string CreatedOn { get; set; }
        public string Remark { get; set; }
        public bool IsLocked { get; set; }
    }
}