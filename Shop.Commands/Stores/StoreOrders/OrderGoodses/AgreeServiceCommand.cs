using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class AgreeServiceCommand:Command<Guid>
    {
        public string ServiceNumber { get; private set; }

        public AgreeServiceCommand() { }
        public AgreeServiceCommand(string serviceNumber)
        {
            ServiceNumber = serviceNumber;
        }
    }
}
