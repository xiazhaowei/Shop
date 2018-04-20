using ENode.Commanding;
using System;

namespace Shop.Commands.Users.ExpressAddresses
{
    public class AddExpressAddressCommand : Command<Guid>
    {
        public string Name { get;private set; }
        public string Mobile { get;private set; }
        public string Region { get;private set; }
        public string Address { get; private set; }
        public string Zip { get; private set; }

        public AddExpressAddressCommand() { }
        public AddExpressAddressCommand(Guid userId,
            string name,
            string mobile,
            string region,
            string address,
            string zip) : base(userId)
        {
            Name = name;
            Mobile = mobile;
            Region = region;
            Address = address;
            Zip = zip;
        }
    }
}
