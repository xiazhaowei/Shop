using ECommon.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Domain.Models.Orders
{
    [Component]
    public class DefaultPricingService : IPricingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="specificationQuantitys"></param>
        /// <param name="shopCash">该订单购物券付款金额</param>
        /// <returns></returns>
        public OrderTotal CalculateTotal(IEnumerable<SpecificationQuantity> specificationQuantitys)
        {
            var orderLines = new List<OrderLine>();

            foreach (var specificationQuantity in specificationQuantitys)
            {
                //遍历每个规格数量
                var lineTotal = Math.Round(specificationQuantity.Specification.Price * specificationQuantity.Quantity, 2);
                orderLines.Add(new OrderLine(specificationQuantity,
                    lineTotal,
                    Math.Round(specificationQuantity.Specification.OriginalPrice * specificationQuantity.Quantity, 2)));
            }
            return new OrderTotal(orderLines.ToArray(),
                orderLines.Sum(x => x.LineTotal),
                orderLines.Sum(x=>x.StoreLineTotal));
        }
    }
}
