using System;

namespace Shop.Commands.Stores.StoreOrders.OrderGoodses
{
    [Serializable]
    public class ServiceExpressInfo
    {
        public string ServiceNumber { get; private set; }
        public string ExpressName { get; private set; }
        public string ExpressNumber { get; private set; }

        public ServiceExpressInfo(string serviceNumber,string expressName,string expressNumber)
        {
            ServiceNumber = serviceNumber;
            ExpressName = expressName;
            ExpressNumber = expressNumber;
        }

    }
}
