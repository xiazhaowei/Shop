using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class DeliverCommand:Command<Guid>
    {
        public string ExpressName { get; private set; }
        public string ExpressCode { get; private set; }
        public string ExpressNumber { get; private set; }

        public DeliverCommand() { }
        public DeliverCommand(string expressName,string expressCode,string expressNumber)
        {
            ExpressName = expressName;
            ExpressCode = expressCode;
            ExpressNumber = expressNumber;
        }
    }
}
