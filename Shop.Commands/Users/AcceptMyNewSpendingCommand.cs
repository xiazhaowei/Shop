using ENode.Commanding;
using System;

namespace Shop.Commands.Users
{
    public class AcceptMyNewSpendingCommand:Command<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }//订单小计
        public decimal StoreAmount { get; private set; }//店铺小计
        public decimal Benevolence { get; private set; }//福豆量
        public decimal HighProfitAmount { get; private set; }//高利润产品额
        public AcceptMyNewSpendingCommand() { }
        public AcceptMyNewSpendingCommand(
            Guid  walletId,
            decimal amount,
            decimal storeAmount,
            decimal benevolence,
            decimal highProfitAmount)
        {
            WalletId = walletId;
            Amount = amount;
            StoreAmount = storeAmount;
            Benevolence = benevolence;
            HighProfitAmount = highProfitAmount;
        }
    }
}
