using ENode.Commanding;
using System;

namespace Shop.Commands.Grantees
{
    /// <summary>
    /// 接受新的帮助
    /// </summary>
    public class AcceptMoneyHelpCommand:Command<Guid>
    {
        public Guid UserId { get;private set; }
        public decimal Amount { get; private set; }
        public string Says { get; private set; }

        public AcceptMoneyHelpCommand() { }
        public AcceptMoneyHelpCommand(Guid userId,decimal amount,string says)
        {
            UserId = userId;
            Amount = amount;
            Says = says;
        }
    }
}
