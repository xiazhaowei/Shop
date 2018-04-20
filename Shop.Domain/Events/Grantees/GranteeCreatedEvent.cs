using ENode.Eventing;
using Shop.Domain.Models.Grantees;
using System;

namespace Shop.Domain.Events.Grantees
{
    /// <summary>
    /// 受助人创建 事件
    /// </summary>
    [Serializable]
    public class GranteeCreatedEvent:DomainEvent<Guid>
    {
        public Guid Publisher { get; private set; }
        public GranteeInfo Info { get; private set; }

        public GranteeCreatedEvent() { }
        public GranteeCreatedEvent(Guid publisher,GranteeInfo info)
        {
            Publisher = publisher;
            Info = info;
        }
    }
}
