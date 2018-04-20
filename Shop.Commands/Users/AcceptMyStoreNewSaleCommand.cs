using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class AcceptMyStoreNewSaleCommand : Command<Guid>
    {
        public Guid StoreOwnerWalletId { get; set; }
        public decimal Amount { get; private set; }

        public AcceptMyStoreNewSaleCommand() { }
        public AcceptMyStoreNewSaleCommand(Guid storeOwnerWalletId, decimal amount)
        {
            StoreOwnerWalletId = storeOwnerWalletId;
            Amount = amount;
        }
    }
}
