using System;

namespace Shop.Domain.Models.OfflineStores
{
    public class SaleLogInfo
    {
        public Guid UserWalletId { get; set; }
        public Guid StoreOwnerWalletId { get; set; }
        public string StoreName { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public decimal StoreAmount { get; set; }
        public decimal UserBenevolence { get; set; }

        public SaleLogInfo(Guid userWalletId,
            Guid storeOwnerWalletId,
            string storeName,
            string region,
            string address,
            decimal amount,
            decimal storeAmount,
            decimal userBenevolence)
        {
            UserWalletId = userWalletId;
            StoreOwnerWalletId = storeOwnerWalletId;
            StoreName = storeName;
            Region = region;
            Address = address;
            Amount = amount;
            StoreAmount = storeAmount;
            UserBenevolence = userBenevolence;
        }
    }
}
