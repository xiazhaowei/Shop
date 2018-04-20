using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.BankCards
{
    public class RemoveBankCardCommand : Command<Guid>
    {
        public Guid BankCardId { get;private set; }

        public RemoveBankCardCommand() { }
        public RemoveBankCardCommand(Guid walletId,Guid bankCardId) : base(walletId) {
            BankCardId = bankCardId;
        }
    }
}
