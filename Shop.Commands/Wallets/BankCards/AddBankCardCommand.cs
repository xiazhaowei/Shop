using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.BankCards
{
    public class AddBankCardCommand : Command<Guid>
    {
        public string BankName { get;private set; }
        public string OwnerName { get; private set; }
        public string Number { get;private set; }

        public AddBankCardCommand() { }
        public AddBankCardCommand(Guid walletId,string bankName,string ownerName,string number) : base(walletId)
        {
            BankName = bankName;
            OwnerName = ownerName;
            Number = number;
        }
    }
}
