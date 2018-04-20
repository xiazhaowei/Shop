using ENode.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    public class ApplyServicesCommand:Command<Guid>
    {
        public ServiceApplyInfo Info { get; private set; }

        public ApplyServicesCommand() { }
        public ApplyServicesCommand(ServiceApplyInfo info)
        {
            Info = info;
        }
    }
}
