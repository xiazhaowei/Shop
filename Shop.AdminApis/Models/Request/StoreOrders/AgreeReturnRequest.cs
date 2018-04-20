using System;

namespace Shop.Api.Models.Request.StoreOrders
{
    public class AgreeReturnRequest
    {
        public Guid Id { get; set; }
        public string Remark { get; set; }
    }
}