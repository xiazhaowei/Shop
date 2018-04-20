using ENode.Commanding;
using System;

namespace Shop.Commands.Users.ExpressAddresses
{
    public class RemoveExpressAddressCommand : Command<Guid>
    {
        public Guid ExpressAddressId { get;private set; }

        public RemoveExpressAddressCommand() { }
        public RemoveExpressAddressCommand(Guid userId,Guid expressAddressId) : base(userId) {
            ExpressAddressId = expressAddressId;
        }
    }
}
