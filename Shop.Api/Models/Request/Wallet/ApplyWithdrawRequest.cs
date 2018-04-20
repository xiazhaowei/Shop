namespace Shop.Api.Models.Request.Wallet
{
    public class ApplyWithdrawRequest
    {
        public decimal Amount { get; set; }

        public BankCardInfo BankCard { get; set; }
    }

    public class BankCardInfo
    {
        public string BankName { get; set; }
        public string Number { get; set; }
        public string OwnerName { get; set; }
    }
}