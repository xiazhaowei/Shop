using ENode.Commanding;
using System;

namespace Shop.Commands.Stores.StoreOrders
{
    public class AgreeReturnCommand:Command<Guid>
    {
        public string Remark { get; private set; }

        public AgreeReturnCommand() { }
        public AgreeReturnCommand(string remark)
        {
            Remark = remark;
        }
    }
}
