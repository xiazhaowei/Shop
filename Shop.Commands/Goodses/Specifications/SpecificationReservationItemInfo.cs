using System;

namespace Shop.Commands.Goodses.Specifications
{
    [Serializable]
    public class SpecificationReservationItemInfo
    {
        public Guid SpecificationId { get;private set; }
        public int Quantity { get;private set; }

        public SpecificationReservationItemInfo(Guid specificationId,int quantity)
        {
            SpecificationId = specificationId;
            Quantity = quantity;
        }
    }
}
