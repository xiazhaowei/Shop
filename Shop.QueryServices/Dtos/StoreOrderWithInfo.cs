using Shop.Common.Enums;
using System;

namespace Shop.QueryServices.Dtos
{
    public class StoreOrderWithInfo
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }

        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }

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
        public decimal OriginalTotal { get; set; }

        public string DeliverExpressName { get; set; }
        public string DeliverExpressCode { get; set; }
        public string DeliverExpressNumber { get; set; }
        public DateTime? DeliverTime { get; set; }
        public string Reason { get; set; }
        public decimal? RefundAmount { get; set; }

        public StoreOrderStatus Status { get; set; }
    }
}
