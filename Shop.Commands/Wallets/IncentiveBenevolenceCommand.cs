using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets
{
    public class IncentiveBenevolenceCommand:Command<Guid>
    {
        public decimal BenevolenceIndex { get; set; }

        public IncentiveBenevolenceCommand() { }
        public IncentiveBenevolenceCommand(Guid walletId,decimal benevolenceIndex):base(walletId)
        {
            BenevolenceIndex = benevolenceIndex;
        }

    }
}
