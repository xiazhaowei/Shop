namespace Shop.Common
{
    /// <summary>
    /// Order 的付款信息
    /// </summary>
    public class PayInfo
    {
        /// <summary>
        /// 付款总金额
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 购物券付款金额
        /// </summary>
        public decimal ShopCash { get; set; }

        public PayInfo(decimal total, decimal shopCash)
        {
            Total = total;
            ShopCash = shopCash;
        }
    }

    /// <summary>
    /// StoreOrder的付款详细信息
    /// </summary>
    public class PayDetailInfo
    {
        public decimal Total { get; set; }//商家订单付款总金额
        public decimal StoreTotal { get; set; }//供应商金额
        public decimal ShopCash { get; set; }//购物券付款金额

        public PayDetailInfo(decimal total,decimal storeTotal,decimal shopCash)
        {
            Total = total;
            StoreTotal = storeTotal;
            ShopCash = shopCash;
        }
    }
}
