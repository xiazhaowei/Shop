using Shop.Common.Enums;

namespace Shop.Api.Models.Request.Wallet
{
    public class WalletPayRequest
    {
        public decimal Amount { get; set; }
        public decimal CashPayAmount { get; set; }
        public decimal ShopCashPayAmount { get; set; }
        public string AccessCode { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
        public bool IsNotVerifyAccessCode { get; set; }
    }
}