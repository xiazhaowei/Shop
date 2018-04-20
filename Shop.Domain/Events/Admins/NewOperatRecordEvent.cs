using ENode.Eventing;
using Shop.Domain.Models.Admins;
using System;

namespace Shop.Domain.Events.Admins
{
    public class NewOperatRecordEvent:DomainEvent<Guid>
    {
        public OperatRecordInfo Info { get; private set; }

        public NewOperatRecordEvent() { }
        public NewOperatRecordEvent(OperatRecordInfo info)
        {
            Info = info;
        }
    }
}
