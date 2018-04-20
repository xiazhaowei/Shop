using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class CashTransfersRequest
    {
        public Guid WalletId { get; set; }
        public CashTransferType Type { get; set; }
        public int Page { get; set; }
    }
}