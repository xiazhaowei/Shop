using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Goodses.Specifications
{
    public class UpdateSpecificationsCommand : Command<Guid>
    {
        public IList<SpecificationInfo> Specifications { get; set; }
        public bool IsAdmin { get; set; }

        public UpdateSpecificationsCommand() { }
        public UpdateSpecificationsCommand(Guid goodsId, IList<SpecificationInfo> specifications,bool isAdmin) : base(goodsId)
        {
            Specifications = specifications;
            IsAdmin = isAdmin;
        }
    }
}
