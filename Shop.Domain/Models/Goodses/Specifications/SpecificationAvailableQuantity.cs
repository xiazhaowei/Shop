using System;

namespace Shop.Domain.Models.Goodses.Specifications
{
    /// <summary>
    /// 规格商品可用数量
    /// </summary>
    public class SpecificationAvailableQuantity
    {
        public SpecificationAvailableQuantity(Guid specificationId, int availableQuantity)
        {
            SpecificationId = specificationId;
            AvailableQuantity = availableQuantity;
        }

        public Guid SpecificationId { get; private set; }
        public int AvailableQuantity { get; private set; }
    }
}
