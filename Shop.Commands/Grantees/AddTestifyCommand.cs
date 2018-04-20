using ENode.Commanding;
using System;

namespace Shop.Commands.Grantees
{
    public class AddTestifyCommand:Command<Guid>
    {
        public Guid UserId { get;private set; }
        public string Relationship { get; private set; }
        public string Name { get; private set; }
        public string Mobile { get; private set; }
        public string CardNumber { get; private set; }
        public string Remark { get; private set; }

        public AddTestifyCommand() { }
        public AddTestifyCommand(Guid userId, 
            string relationship, 
            string name, 
            string mobile,
            string cardNumber,
            string remark)
        {
            UserId = userId;
            Relationship = relationship;
            Name = name;
            Mobile = mobile;
            CardNumber = cardNumber;
            Remark = remark;
        }
    }
}
