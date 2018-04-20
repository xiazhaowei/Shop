using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户新消费
    /// </summary>
    [Serializable]
    public class UserNewSpendingEvent:DomainEvent<Guid>
    {
        public decimal Amount { get; private set; }

        public UserNewSpendingEvent() { }
        public UserNewSpendingEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
