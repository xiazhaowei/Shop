using System;

namespace Shop.Domain.Models.Partners
{
    public class BalanceLogInfo
    {
        public Guid WalletId { get; set; }
        public string Region { get; set; }
        public decimal Amount { get; set; }//销售额
        public decimal BalanceAmount { get; set; }
        public decimal CashAmount { get; set; }
        public decimal BenevolenceAmount { get; set; }

        public BalanceLogInfo(Guid walletId,
            string region,
            decimal amount,
            decimal balanceAmount,
            decimal cashAmount,
            decimal benevolenceAmount)
        {
            WalletId = walletId;
            Region = region;
            Amount = amount;
            BalanceAmount = balanceAmount;
            CashAmount = cashAmount;
            BenevolenceAmount = benevolenceAmount;
        }
    }
}
