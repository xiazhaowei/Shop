using Shop.Common.Enums;
using System;
using System.ComponentModel;

namespace Shop.QueryServices.Dtos
{
    public class ShopCashTransfer
    {
        public Guid Id { get; set; }
        private Guid WalletId;//钱包Id
        public string Number { get; set; }
        public decimal Amount { get;  set; }
        public decimal Fee { get;  set; }
        public decimal FinallyValue { get; set; }
        public ShopCashTransferType Type { get; set; }
        public ShopCashTransferStatus Status { get; set; }
        public WalletDirection Direction { get;  set; }
        public DateTime CreatedOn { get; set; }
        public string Remark { get;  set; }
    }
    
}
