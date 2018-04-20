using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.ShopCashTransfers
{
    public class SetShopCashTransferSuccessCommand:Command<Guid>
    {
        public decimal FinallyValue { get; private set; }

        public SetShopCashTransferSuccessCommand() { }
        public SetShopCashTransferSuccessCommand(Guid id,decimal finallyValue) : base(id)
        {
            FinallyValue = finallyValue;
        }
    }
}
