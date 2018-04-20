using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class IncentiveWalletRequest
    {
        public Guid WalletId { get; set; }
        public decimal BenevolenceIndex { get; set; }
    }
}