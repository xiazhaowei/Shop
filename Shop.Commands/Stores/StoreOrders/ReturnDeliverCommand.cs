using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class ReturnDeliverCommand:Command<Guid>
    {
        public string ExpressName { get; private set; }
        public string ExpressCode { get; private set; }
        public string ExpressNumber { get; private set; }

        public ReturnDeliverCommand() { }
        public ReturnDeliverCommand(string expressName,string expressCode,string expressNumber)
        {
            ExpressName = expressName;
            ExpressCode = expressCode;
            ExpressNumber = expressNumber;
        }
    }
}
