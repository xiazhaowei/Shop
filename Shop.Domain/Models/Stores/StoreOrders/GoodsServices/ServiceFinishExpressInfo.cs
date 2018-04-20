namespace Shop.Domain.Models.Stores.StoreOrders.GoodsServices
{
    /// <summary>
    /// 维修返回给用户快递信息
    /// </summary>
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
