using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class DisAgreeRefundCommand : Command<Guid>
    {
        public string ServiceNumber { get; private set; }

        public DisAgreeRefundCommand() { }
        public DisAgreeRefundCommand(string serviceNumber)
        {
            ServiceNumber = serviceNumber;
        }
    }
}
