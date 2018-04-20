using ENode.Commanding;
using System;

namespace Shop.Commands.Partners
{
    public class AcceptNewBalanceCommand : Command<Guid>
    {
        public decimal Amount { get; private set; }

        public AcceptNewBalanceCommand() { }
        public AcceptNewBalanceCommand(decimal amount)
        {
            Amount = amount;
        }
    }
}
