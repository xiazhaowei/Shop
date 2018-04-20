using ENode.Eventing;
using Shop.Domain.Models.OfflineStores;
using System;

namespace Shop.Domain.Events.OfflineStores
{
    [Serializable]
    public class NewSaleAcceptedEvent:DomainEvent<Guid>
    {
        public Guid StoreOwnerId { get; set; }
        public Guid StoreOwnerWalletId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserWalletId { get; set; }
        public StatisticInfo Info { get; private set; }
        public decimal Amount { get; set; }
        public decimal StoreAmount { get; set; }
        public decimal BenevolenceAmount { get; set; }

        public NewSaleAcceptedEvent() { }
        public NewSaleAcceptedEvent(
            Guid storeOwnerId,
            Guid storeOwnerWalletId,
            Guid userId,
            Guid userWalletId,
            decimal amount,
            decimal storeAmount,
            decimal benevolenceAmount,
            StatisticInfo statisticInfo)
        {
            StoreOwnerId = storeOwnerId;
            StoreOwnerWalletId = storeOwnerWalletId;
            UserId = userId;
            UserWalletId = userWalletId;
            Amount = amount;
            StoreAmount = storeAmount;
            BenevolenceAmount = benevolenceAmount;
            Info = statisticInfo;
        }
    }
}
