using System;
using ENode.Commanding;
using Shop.Common.Enums;

namespace Shop.Commands.Stores
{
    public class UpdateStoreStautsCommand: Command<Guid>
    {
        public StoreStatus Status { get; private set; }

        public UpdateStoreStautsCommand() { }
        public UpdateStoreStautsCommand(StoreStatus status)
        {
            Status = status;
        }
    }
}
