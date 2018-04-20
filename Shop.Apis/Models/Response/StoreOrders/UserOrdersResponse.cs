using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.StoreOrders
{
    public class UserOrdersResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<StoreOrder> StoreOrders { get; set; }
    }

    public class StoreOrder
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
        public string Region { get; set; }
        public string Number { get; set; }
        public string Remark { get; set; }
        public string ExpressRegion { get; set; }
        public string ExpressAddress { get; set; }
        public string ExpressName { get; set; }
        public string ExpressMobile { get; set; }
        public string ExpressZip { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public string Status { get; set; }

        public string DeliverExpressName { get; set; }
        public string DeliverExpressCode { get; set; }
        public string DeliverExpressNumber { get; set; }

        public string ReturnDeliverExpressName { get; set; }
        public string ReturnDeliverExpressCode { get; set; }
        public string ReturnDeliverExpressNumber { get; set; }

        public string Reason { get; set; }
        public decimal RefundAmount { get; set; }
        public string StoreRemark { get; set; }

        public IList<StoreOrderGoods> StoreOrderGoodses { get; set; } 
    }

    public class StoreOrderGoods
    {
        public Guid Id { get; set; }
        public Guid GoodsId { get; set; }
        public Guid SpecificationId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsPic { get; set; }
        public string SpecificationName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public decimal Benevolence { get; set; }
    }


}