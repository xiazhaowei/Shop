using System;

namespace Shop.Api.Models.Request.Partners
{
    public class AcceoptNewBalanceRequest
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 地区销售额
        /// </summary>
        public decimal Amount { get; set; }
    }
}