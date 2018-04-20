using ENode.Commanding;
using System;


namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class AddServicesExpressInfoCommand : Command<Guid>
    {
        public ServiceExpressInfo Info { get; private set; }

        public AddServicesExpressInfoCommand() { }
        public AddServicesExpressInfoCommand(ServiceExpressInfo info)
        {
            Info = info;
        }
    }
}
