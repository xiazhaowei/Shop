using ENode.Commanding;
using System;

namespace Shop.Commands.BenevolenceIndexs
{
    public class CreateBenevolenceIndexCommand:Command<Guid>
    {
        public decimal BenevolenceIndex { get; private set; }
        public decimal BenevolenceAmount { get; private set; }

        public CreateBenevolenceIndexCommand() { }
        public CreateBenevolenceIndexCommand(Guid id,decimal benevolenceIndex,decimal benevolenceAmount):base(id)
        {
            BenevolenceIndex = benevolenceIndex;
            BenevolenceAmount = benevolenceAmount;
        }
    }
}
