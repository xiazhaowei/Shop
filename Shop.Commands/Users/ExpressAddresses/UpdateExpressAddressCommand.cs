using ENode.Commanding;
using System;

namespace Shop.Commands.Users.ExpressAddresses
{
    public class UpdateExpressAddressCommand : Command<Guid>
    {
        public Guid ExpressAddressId { get;private set; }
        public string Name { get; private set; }
        public string Region { get; private set; }
        public string Address { get; private set; }
        public string Mobile { get; private set; }
        public string Zip { get; private set; }

        public UpdateExpressAddressCommand() { }
        public UpdateExpressAddressCommand(Guid userId,
            Guid expressAddressId,
            string name,
            string mobile,
            string region,
            string address,
            string zip) : base(userId)
        {
            ExpressAddressId = expressAddressId;
            Name = name;
            Mobile = mobile;
            Region = region;
            Address = address;
            Zip = zip;
        }
    }
}
