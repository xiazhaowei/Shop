using System;

namespace Shop.QueryServices.Dtos
{
    public class WalletAlis
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string Portrait { get; set; }

        public decimal Cash { get; set; }
        public decimal ShopCash { get; set; }
        public decimal Benevolence { get; set; }
        public decimal Earnings { get; set; }
    }
}
