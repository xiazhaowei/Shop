using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Wallet
{
    public class AddCashTransferRequest
    {
        /// <summary>
        /// 钱包id
        /// </summary>
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }

        public WalletDirection Direction { get; set; }
    }
}