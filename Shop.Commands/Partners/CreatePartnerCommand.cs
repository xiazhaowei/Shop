using ENode.Commanding;
using Shop.Common.Enums;
using System;

namespace Shop.Commands.Partners
{
    public class CreatePartnerCommand:Command<Guid>
    {
        public Guid UserId { get; private set; }
        public Guid WalletId { get; private set; }
        public string Mobile { get; private set; }
        public string Region { get; private set; }
        public PartnerLevel Level { get; private set; }
        public decimal Persent { get; private set; }
        public decimal CashPersent { get; private set; }
        public int BalanceInterval { get; private set; }
        public string Remark { get; private set; }
        public bool IsLocked { get; set; }

        public CreatePartnerCommand() { }
        public CreatePartnerCommand(Guid id,
            Guid userId,
            Guid walletId,
            string mobile,
            string region,
            PartnerLevel level,
            decimal persent,
            decimal cashPersent,
            int balanceInterval,
            string remark,
            bool isLocked):base(id)
        {
            UserId = userId;
            WalletId = walletId;
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
