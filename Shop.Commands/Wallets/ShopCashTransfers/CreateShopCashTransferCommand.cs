using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Wallets.ShopCashTransfers
{
    public class CreateShopCashTransferCommand:Command<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Fee { get; private set; }
        public WalletDirection Direction { get; private set; }
        public string Remark { get; private set; }
        public string Number { get; private set; }
        public ShopCashTransferType Type { get; private set; }
        public ShopCashTransferStatus Status { get; private set; }


        private CreateShopCashTransferCommand() { }
        public CreateShopCashTransferCommand(Guid id,
            Guid walletId, 
            string number, 
            ShopCashTransferType type,
            ShopCashTransferStatus status,
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
