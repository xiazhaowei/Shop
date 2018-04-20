using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Wallets.CashTransfers
{
    public class CreateCashTransferCommand:Command<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Fee { get; private set; }
        public WalletDirection Direction { get; private set; }
        public string Remark { get; private set; }
        public string Number { get; private set; }
        public CashTransferType Type { get; private set; }
        public CashTransferStatus Status { get; private set; }


        private CreateCashTransferCommand() { }
        public CreateCashTransferCommand(Guid id,
            Guid walletId, 
            string number, 
            CashTransferType type,
            CashTransferStatus status,
            decimal amount, 
            decimal fee, 
            WalletDirection direction,
            string remark) : base(id)
        {
            WalletId = walletId;
            Number = number;
            Type = type;
            Status = status;
            Amount = amount;
            Fee = fee;
            Direction = direction;
            Remark = remark;
        }
    }
    
}
