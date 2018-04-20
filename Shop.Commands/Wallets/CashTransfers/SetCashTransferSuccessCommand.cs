using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.CashTransfers
{
    public class SetCashTransferSuccessCommand:Command<Guid>
    {
        public decimal FinallyValue { get;private set; }

        public SetCashTransferSuccessCommand() { }
        public SetCashTransferSuccessCommand(Guid id,decimal finallyValue) : base(id) {
            FinallyValue = finallyValue;
        }
    }
}
