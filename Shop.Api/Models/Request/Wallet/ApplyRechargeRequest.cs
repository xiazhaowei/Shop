namespace Shop.Api.Models.Request.Wallet
{
    public class ApplyRechargeRequest
    {
        public decimal Amount { get; set; }
        public string Pic { get; set; }
        public BankCardInfo BankCard { get; set; }
    }

   
}