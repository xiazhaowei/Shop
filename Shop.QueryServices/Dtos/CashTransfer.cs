using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class CashTransfer
    {
        public Guid Id { get; set; }
        private Guid WalletId;//钱包Id
        public string Number { get; set; }
        public decimal Amount { get;  set; }
        public decimal Fee { get; private set; }
        public decimal FinallyValue { get; set; }
        public CashTransferType Type { get; set; }
        public CashTransferStatus Status { get; set; }
        public WalletDirection Direction { get;  set; }
        public DateTime CreatedOn { get; set; }
        public string Remark { get;  set; }
    }
    
}
