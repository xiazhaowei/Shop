using Shop.Common.Enums;

namespace Shop.Domain.Models.Wallets.CashTransfers
{
    /// <summary>
    /// 现金记录 信息
    /// </summary>
    public class CashTransferInfo
    {
        public decimal Amount { get;private set; }
        public decimal Fee { get; private set; }
        public WalletDirection Direction{get; private set; }
        public string Remark { get; private set; }

        public CashTransferInfo(decimal amount,decimal fee,WalletDirection direction,string remark)
        {
            Amount = amount;
            Fee = fee;
            Direction = direction;
            Remark = remark;
        }
    }

   
}
