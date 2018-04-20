using ENode.Commanding;
using System;

namespace Shop.Commands.Wallets
{
    public class CreateWalletCommand:Command<Guid>
    {
        public Guid UserId { get;private set; }

        public CreateWalletCommand() { }
        public CreateWalletCommand(Guid id,Guid userId):base(id)
        {
            UserId = userId;
        }
    }
}
