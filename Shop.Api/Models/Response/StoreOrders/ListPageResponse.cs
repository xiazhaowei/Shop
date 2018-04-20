using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.StoreOrders
{
    public class ListPageResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<StoreOrderWithInfo> StoreOrders { get; set; }
    }
    public class StoreOrderWithInfo
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
        public string Mobile { get; set; }
        public string NickName { get; set; }

        public string Name { get; set; }
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

        public IList<StoreOrderGoods> StoreOrderGoodses { get; set; }
    }
}