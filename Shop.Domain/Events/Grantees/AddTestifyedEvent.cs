using ENode.Eventing;
using Shop.Domain.Models.Grantees;
using System;

namespace Shop.Domain.Events.Grantees
{
    /// <summary>
    /// 添加做证人
    /// </summary>
    [Serializable]
    public class AddTestifyedEvent:DomainEvent<Guid>
    {
        public Testify Testify { get; private set; }

        public AddTestifyedEvent() { }
        public AddTestifyedEvent(Testify testify)
        {
            Testify = testify;
        }
    }
}
