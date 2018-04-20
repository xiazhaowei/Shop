﻿using ENode.Eventing;
using System;

namespace Shop.Domain.Events.Stores.StoreOrders.GoodsServices
{
    [Serializable]
    public class ServiceFinishedEvent : DomainEvent<Guid>
    {
        public Guid WalletId { get; private set; }
        public Guid StoreOwnerWalletId { get; private set; }
        public decimal Total { get; private set; }
        public decimal Benevolence { get; private set; }

        public ServiceFinishedEvent() { }
        public ServiceFinishedEvent(
            Guid walletId,
            Guid storeOwnerWalletId,
            decimal total,
            decimal benevolence)
        {
            WalletId = walletId;
            StoreOwnerWalletId = storeOwnerWalletId;
            Total = total;
            Benevolence = benevolence;
        }
    }
}
