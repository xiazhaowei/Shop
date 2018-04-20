using ENode.Commanding;
using System;

namespace Shop.Commands.Stores
{
    public class AcceptNewStoreOrderCommand:Command<Guid>
    {
        public Guid StoreOrderId { get; private set; }

        private AcceptNewStoreOrderCommand() { }
        public AcceptNewStoreOrderCommand(Guid id, Guid storeOrderId) : base(id)
        {
            StoreOrderId = storeOrderId;
        }
    }
}
