using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets.BankCards
{
    public class UpdateBankCardCommand : Command<Guid>
    {
        public Guid BankCardId { get;private set; }
        public string BankName { get; private set; }
        public string OwnerName { get; private set; }
        public string Number { get; private set; }


        public UpdateBankCardCommand() { }
        public UpdateBankCardCommand(Guid walletId, Guid bankCardId,string bankName,string ownerName,string number) : base(walletId)
        {
            BankCardId = bankCardId;
            BankName = bankName;
            OwnerName = ownerName;
            Number = number;
        }
    }
}
