using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets
{
    public class AcceptNewBenevolenceTransferCommand:Command<Guid>
    {
        public Guid TransferId { get; private set; }

        private AcceptNewBenevolenceTransferCommand() { }
        public AcceptNewBenevolenceTransferCommand(Guid id, Guid transferId) : base(id)
        {
            TransferId = transferId;
        }
    }
}
