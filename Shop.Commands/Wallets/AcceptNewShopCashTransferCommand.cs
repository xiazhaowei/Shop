using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets
{
    public class AcceptNewShopCashTransferCommand:Command<Guid>
    {
        public Guid TransferId { get; private set; }

        private AcceptNewShopCashTransferCommand() { }
        public AcceptNewShopCashTransferCommand(Guid id, Guid transferId) : base(id)
        {
            TransferId = transferId;
        }
    }
}
