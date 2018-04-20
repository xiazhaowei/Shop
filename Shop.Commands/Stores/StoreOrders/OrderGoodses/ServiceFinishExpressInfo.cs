using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    [Serializable]
    public class ServiceFinishExpressInfo
    {
        public string ExpressName { get;private set; }
        public string ExpressNumber { get;private set; }

        public ServiceFinishExpressInfo(string expressName,string expressNumber)
        {
            ExpressName = expressName;
            ExpressNumber = expressNumber;
        }
    }
}
