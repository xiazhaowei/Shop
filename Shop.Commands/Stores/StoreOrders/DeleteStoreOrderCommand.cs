using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class DeleteStoreOrderCommand:Command<Guid>
    {
        public DeleteStoreOrderCommand() { }
        public DeleteStoreOrderCommand(Guid id):base(id) { }
    }
}
