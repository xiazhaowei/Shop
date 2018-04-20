namespace Shop.Domain.Models.Stores.StoreOrders
{
    public class ExpressInfo
    {
        public string ExpressName { get;private set; }
        public string ExpressCode { get; private set; }
        public string ExpressNumber { get;private set; }

        public ExpressInfo(string expressName,string expressCode,string expressNumber)
        {
            ExpressName = expressName;
            ExpressCode = expressCode;
            ExpressNumber = expressNumber;
        }
    }
}
