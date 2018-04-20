using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class MyParentCanGetGratefulAwardEvent:DomainEvent<Guid>
    {
        public Guid ParentId { get; set; }
        public decimal Amount { get; set; }
        public string Remak { get; set; }

        public MyParentCanGetGratefulAwardEvent() { }
        public MyParentCanGetGratefulAwardEvent(Guid parentId,decimal amount,string remark)
        {
            ParentId = parentId;
            Amount = amount;
            Remak = remark;
        }
    }
}
