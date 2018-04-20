using Shop.Domain.Models.Wallets;
using System;

namespace Shop.Domain.Events.Wallets
{
    [Serializable]
    public class NewCashTransferAcceptedEvent:WalletEvent
    {
        public Guid TransferId { get; private set; }
        public decimal FinallyValue { get; private set; }
        public WalletStatisticInfo StatisticInfo { get; private set; }

        public NewCashTransferAcceptedEvent() { }
        public NewCashTransferAcceptedEvent(Guid userId,Guid transferId,decimal finallyValue,WalletStatisticInfo statisticInfo) :base(userId)
        {
            TransferId = transferId;
            FinallyValue = finallyValue;
            StatisticInfo = statisticInfo;
        }
    }
}
