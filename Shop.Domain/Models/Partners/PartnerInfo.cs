using Shop.Common.Enums;

namespace Shop.Domain.Models.Partners
{
    public class PartnerInfo
    {
        public string Mobile { get; set; }
        public string Region { get;private set; }
        public PartnerLevel Level { get; private set; }
        public decimal Persent { get; private set; }
        public decimal CashPersent { get; private set; }
        public int BalanceInterval { get; private set; }//结算周期
        public string Remark { get; set; }
        public bool IsLocked { get; private set; }

        public PartnerInfo(
            string mobile,
            string region,
            PartnerLevel level,
            decimal persent,
            decimal cashPersent,
            int balanceInterval,
            string remark,
            bool isLocked)
        {
            Mobile = mobile;
            Region = region;
            Level = level;
            Persent = persent;
            CashPersent = cashPersent;
            BalanceInterval = balanceInterval;
            Remark = remark;
            IsLocked = isLocked;
        }
    }
}
