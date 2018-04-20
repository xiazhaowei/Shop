using System;

namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 预定项目 规格-数量
    /// </summary>
    public class ReservationItem
    {
        public ReservationItem(Guid specificationId, int quantity)
        {
            SpecificationId = specificationId;
            Quantity = quantity;
        }

        public Guid SpecificationId { get; private set; }
        public int Quantity { get; private set; }
    }
}
