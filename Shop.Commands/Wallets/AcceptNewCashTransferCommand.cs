using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets
{
    public class AcceptNewCashTransferCommand:Command<Guid>
    {
        public Guid TransferId { get; private set; }

        private AcceptNewCashTransferCommand() { }
        public AcceptNewCashTransferCommand(Guid id, Guid transferId) : base(id)
        {
            TransferId = transferId;
        }
    }
}
