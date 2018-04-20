using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.StoreOrders
{
    public class InfoResponse:BaseApiResponse
    {
        public Guid Id { get; set; }
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
        public string Status { get; set; }
        public IList<StoreOrderGoods> StoreOrderGoodses { get; set; }
    }
}