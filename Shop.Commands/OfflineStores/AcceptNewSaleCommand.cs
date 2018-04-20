using ENode.Commanding;
using System;

namespace Shop.Commands.OfflineStores
{
    public class AcceptNewSaleCommand:Command<Guid>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }

        public AcceptNewSaleCommand() { }
        public AcceptNewSaleCommand(Guid userId,decimal amount)
        {
            UserId = userId;
            Amount = amount;
        }
    }
}
