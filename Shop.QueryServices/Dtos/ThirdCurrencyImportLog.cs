using System;

namespace Shop.QueryServices.Dtos
{
    public class ThirdCurrencyImportLog
    {
        public Guid Id { get; set; }
        public Guid ThirdCurrencyId { get; set; }
        public Guid WalletId { get; set; }
        public string Mobile { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public decimal ShopCashAmount { get; set; }
        public decimal Conversion { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
