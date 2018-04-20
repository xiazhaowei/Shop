using System;

namespace Shop.Api.Models.Request.Payment
{
    public class ProcessorPaymentRequest
    {
        public Guid PaymentId { get; set; }
        /// <summary>
        /// 付款总金额
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 购物券付款金额
        /// </summary>
        public decimal ShopCash { get; set; }
    }
}