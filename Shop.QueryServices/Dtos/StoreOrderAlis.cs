using Shop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.QueryServices.Dtos
{
    public class StoreOrderAlis
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid UserId { get; set; }

        public string Region { get; set; }
        public string Number { get; set; }

        public string ExpressRegion { get; set; }


        public DateTime CreatedOn { get; set; }
        public decimal Total { get; set; }
        public decimal ShopCash { get; set; }
        public decimal StoreTotal { get; set; }
        public decimal OriginalTotal { get; set; }

        public StoreOrderStatus Status { get; set; }
    }
}
