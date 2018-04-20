using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class MyParentCanGetBenevolenceCommand:Command<Guid>
    {
        public decimal BenevolenceAmount { get; set; }

        public MyParentCanGetBenevolenceCommand() { }
        public MyParentCanGetBenevolenceCommand(decimal benevolenceAmount)
        {
            BenevolenceAmount = benevolenceAmount;
        }
    }
}
