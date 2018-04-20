using ENode.Commanding;
using Shop.Common;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Orders
{
    public class PlaceOrderCommand : Command<Guid>
    {
        public Guid UserId { get;private set; }
        public ExpressAddressInfo ExpressAddressInfo { get;private set; }
        public IList<SpecificationInfo> Specifications { get;private set; }

        public PlaceOrderCommand() { }
        public PlaceOrderCommand(Guid id,
            Guid userId,
            ExpressAddressInfo expressAddressInfo,
            IList<SpecificationInfo> specifications) : base(id)
        {
            UserId = userId;
            ExpressAddressInfo = expressAddressInfo;
            Specifications = specifications;
        }
    }
}
