using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    public class UserSpendingHighProfitGoodsEvent:DomainEvent<Guid>
    {
        public decimal Amount { get; set; }

        public UserSpendingHighProfitGoodsEvent() { }
        public UserSpendingHighProfitGoodsEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
