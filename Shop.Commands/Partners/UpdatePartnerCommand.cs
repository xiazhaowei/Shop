using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Partners
{
    public class UpdatePartnerCommand:Command<Guid>
    {
        public string Mobile { get;private set; }
        public string Region { get; private set; }
        public PartnerLevel Level { get; private set; }
        public decimal Persent { get; private set; }
        public decimal CashPersent { get; private set; }
        public int BalanceInterval { get; private set; }
        public string Remark { get; private set; }
        public bool IsLocked { get; private set; }

        public UpdatePartnerCommand() { }
        public UpdatePartnerCommand(
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
