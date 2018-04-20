using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Users
{
    /// <summary>
    /// 用户获取店铺销售善心激励
    /// </summary>
    [Serializable]
    public class UserGetSaleBenevolenceEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }

        public UserGetSaleBenevolenceEvent() { }
        public UserGetSaleBenevolenceEvent(Guid walletId,decimal amount)
        {
            WalletId = walletId;
            Amount = amount;
        }
    }
}
