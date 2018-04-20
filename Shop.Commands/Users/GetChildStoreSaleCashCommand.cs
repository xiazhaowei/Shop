using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class GetChildStoreSaleCashCommand:Command<Guid>
    {
        public decimal Amount { get; set; }

        public GetChildStoreSaleCashCommand() { }
        public GetChildStoreSaleCashCommand(Guid id,decimal amount):base(id)
        {
            Amount = amount;
        }
    }
}
