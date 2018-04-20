using Shop.Common.Enums;

namespace Shop.Domain.Models.Wallets.BenevolenceTransfers
{
    /// <summary>
    /// 善心记录 信息
    /// </summary>
    public class BenevolenceTransferInfo
    {
        public decimal Amount { get;private set; }
        public decimal Fee { get; private set; }
        public WalletDirection Direction{get; private set; }
        public string Remark { get; private set; }

        public BenevolenceTransferInfo(decimal amount,decimal fee,WalletDirection direction,string remark)
        {
            Amount = amount;
            Fee = fee;
            Direction = direction;
            Remark = remark;
        }
    }

    
}
