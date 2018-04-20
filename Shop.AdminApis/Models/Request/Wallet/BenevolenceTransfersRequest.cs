using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class BenevolenceTransfersRequest
    {
        public Guid WalletId { get; set; }
        public BenevolenceTransferType Type { get; set; }
        public int Page { get; set; }
    }
}