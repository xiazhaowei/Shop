using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class GetChildStoreSaleBenevolenceCommand:Command<Guid>
    {
        public decimal Amount { get; set; }

        public GetChildStoreSaleBenevolenceCommand() { }
        public GetChildStoreSaleBenevolenceCommand(Guid id,decimal amount):base(id)
        {
            Amount = amount;
        }
    }
}
