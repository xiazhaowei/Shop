using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class AcceptChildGratefulAwardCommand:Command<Guid>
    {
        public decimal Amount { get; set; }
        public string Remark { get; set; }

        public AcceptChildGratefulAwardCommand() { }
        public AcceptChildGratefulAwardCommand(Guid id,decimal amount,string remark) : base(id)
        {
            Amount = amount;
            Remark = remark;
        }
    }
}
