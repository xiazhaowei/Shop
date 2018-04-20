using System.Collections.Generic;

namespace Shop.Domain.Models.Orders
{
    /// <summary>
    /// 订单小计计算
    /// </summary>
    public interface IPricingService
    {
        OrderTotal CalculateTotal(IEnumerable<SpecificationQuantity> specifications);
    }
}
