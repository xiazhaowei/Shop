using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.BenevolenceTransfers
{
    public class SetBenevolenceTransferSuccessCommand : Command<Guid>
    {
        public decimal FinallyValue { get; private set; }
        public SetBenevolenceTransferSuccessCommand() { }
        public SetBenevolenceTransferSuccessCommand(Guid id,decimal finallyValue) : base(id)
        {
            FinallyValue = finallyValue;
        }
    }
}
