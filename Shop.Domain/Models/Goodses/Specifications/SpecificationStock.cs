using System;

namespace Shop.Domain.Models.Goodses.Specifications
{
    /// <summary>
    /// 规格库存
    /// </summary>
    public class SpecificationStock
    {
        public SpecificationStock(Guid specificationId, int stock)
        {
            SpecificationId = specificationId;
            Stock = stock;
        }

        public Guid SpecificationId { get; private set; }
        /// <summary>
        /// 新库存数量
        /// </summary>
        public int Stock { get; private set; }
    }
}
