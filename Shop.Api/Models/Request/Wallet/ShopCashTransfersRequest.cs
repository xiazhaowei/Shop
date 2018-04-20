using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class ShopCashTransfersRequest
    {
        public Guid WalletId { get; set; }
        public ShopCashTransferType Type { get; set; }
        public int Page { get; set; }
    }
}