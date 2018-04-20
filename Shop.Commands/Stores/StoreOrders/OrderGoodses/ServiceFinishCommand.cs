using ENode.Commanding;
using System;


namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class ServiceFinishCommand : Command<Guid>
    {
        public ServiceFinishExpressInfo Info { get;private set; }

        public ServiceFinishCommand() { }
        public ServiceFinishCommand(ServiceFinishExpressInfo info)
        {
            Info = info;
        }
    }
}
