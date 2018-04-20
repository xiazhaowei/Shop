using ENode.Commanding;
using System;

namespace Shop.Commands.Admins
{
    public class NewOperatRecordCommand:Command<Guid>
    {
        public string Operat { get; private set; }
        public Guid AboutId { get; private set; }
        public string Remark { get; private set; }

        public NewOperatRecordCommand() { }
        public NewOperatRecordCommand(string operat,Guid aboutId,string remark)
        {
            Operat = operat;
            AboutId = aboutId;
            Remark = remark;
        }
    }
}
