using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.ThirdCurrencys
{
    public class AdjustMaxImportAmountRequest
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public WalletDirection Direction { get; set; }
    }
}