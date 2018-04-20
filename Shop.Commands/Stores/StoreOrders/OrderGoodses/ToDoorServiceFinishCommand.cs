using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class ToDoorServiceFinishCommand : Command<Guid>
    {
        public string ServiceNumber { get; private set; }

        public ToDoorServiceFinishCommand() { }
        public ToDoorServiceFinishCommand(string serviceNumber)
        {
            ServiceNumber = serviceNumber;
        }
    }
}
