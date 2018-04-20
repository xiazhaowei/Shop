using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Goodses.Specifications
{
    public class AddSpecificationsCommand:Command<Guid>
    {
        public IList<SpecificationInfo> Specifications { get; set; }

        public AddSpecificationsCommand() { }
        public AddSpecificationsCommand(Guid goodsId, IList<SpecificationInfo> specifications) : base(goodsId)
        {
            Specifications = specifications;
        }
    }
}
