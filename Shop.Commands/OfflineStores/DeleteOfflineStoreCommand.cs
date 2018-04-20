using ENode.Commanding;
using System;

namespace Shop.Commands.OfflineStores
{
    public class DeleteOfflineStoreCommand:Command<Guid>
    {
        public DeleteOfflineStoreCommand() { }
        public DeleteOfflineStoreCommand(Guid id):base(id) { }
    }
}
