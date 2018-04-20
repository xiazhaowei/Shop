using System;
using ENode.Commanding;

namespace Shop.Commands.Stores
{
    public class UpdateReturnAddressCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Mobile { get;private  set; }
        public string Address { get;private set; }

        public UpdateReturnAddressCommand() { }
        public UpdateReturnAddressCommand(string name,string mobile,string address)
        {
            Name = name;
            Mobile = mobile;
            Address = address;
        }
    }
}
