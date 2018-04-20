using ENode.Eventing;
using Shop.Domain.Models.Partners;
using System;

namespace Shop.Domain.Events.Partners
{
    [Serializable]
    public class AcceptedNewBalanceEvent:DomainEvent<Guid>
    {
        public Guid WalletId { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal CashBalanceAmount { get; set; }
        public decimal BenevolenceBalanceAmount { get; set; }
        public PartnerStatisticInfo StatisticInfo { get; set; }

        public AcceptedNewBalanceEvent() { }
        public AcceptedNewBalanceEvent(Guid walletId,
            decimal cashBalanceAmount,
            decimal benevolenceBalanceAmount, 
            decimal balanceAmount,
            PartnerStatisticInfo statisticInfo)
        {
            WalletId = walletId;
            CashBalanceAmount = cashBalanceAmount;
            BenevolenceBalanceAmount = benevolenceBalanceAmount;
            BalanceAmount = balanceAmount;
            StatisticInfo = statisticInfo;
        }
    }
}
