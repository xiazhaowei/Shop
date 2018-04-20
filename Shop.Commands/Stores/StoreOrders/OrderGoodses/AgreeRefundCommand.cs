using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class AgreeRefundCommand : Command<Guid>
    {
        public string ServiceNumber { get; private set; }

        public AgreeRefundCommand() { }
        public AgreeRefundCommand(string serviceNumber)
        {
            ServiceNumber = serviceNumber;
        }
    }
}
