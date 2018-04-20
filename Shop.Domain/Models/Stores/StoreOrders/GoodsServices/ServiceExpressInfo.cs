using System;

namespace Shop.Domain.Models.Stores.StoreOrders.GoodsServices
{
    /// <summary>
    /// 商品退货发货信息
    /// </summary>
    [Serializable]
    public class ServiceExpressInfo
    {
        /// <summary>
        /// 服务号
        /// </summary>
        public string ServiceNumber { get;private set; }
        public string ExpressName { get;private set; }
        public string ExpressNumber { get;private set; }

        public ServiceExpressInfo(string serviceNumber,string expressName,string expressNumber)
        {
            ServiceNumber = serviceNumber;
            ExpressName = expressName;
            ExpressNumber = expressNumber;
        }
    }
}
