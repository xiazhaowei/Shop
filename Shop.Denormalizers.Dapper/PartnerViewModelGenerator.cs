using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Partners;
using System.Threading.Tasks;
using Xia.Common;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class PartnerViewModelGenerator:BaseGenerator,
        IMessageHandler<PartnerCreatedEvent>,
        IMessageHandler<PartnerUpdatedEvent>,
        IMessageHandler<PartnerDeletedEvent>,
        IMessageHandler<PartnerStatisticInfoChangedEvent>,
        IMessageHandler<AcceptedNewBalanceEvent>,
        IMessageHandler<NewBalanceLogEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(PartnerCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    WalletId = evnt.WalletId,
                    Mobile=evnt.Info.Mobile,
                    Region = evnt.Info.Region,
                    Level = (int)evnt.Info.Level,
                    Persent=evnt.Info.Persent,
                    CashPersent = evnt.Info.CashPersent,
                    BalanceInterval=evnt.Info.BalanceInterval,
                    LastCashBalancedAmount=0,
                    LastBenevolenceBalancedAmount=0,
                    LastBalancedAmount =0,
                    TotalCashBalancedAmount=0,
                    TotalBenevolenceBalancedAmount=0,
                    TotalBalancedAmount =0,
                    BalancedDate=evnt.Timestamp,
                    CreatedOn = evnt.Timestamp,
                    Remark=evnt.Info.Remark,
                    IsLocked=evnt.Info.IsLocked,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.PartnerTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AcceptedNewBalanceEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    LastCashBalancedAmount=evnt.StatisticInfo.LastCashBalancedAmount,
                    LastBenevolenceBalancedAmount=evnt.StatisticInfo.LastBenevolenceBalancedAmount,
                    LastBalancedAmount=evnt.StatisticInfo.LastBalancedAmount,
                    TotalCashBalancedAmount=evnt.StatisticInfo.TotalCashBalancedAmount,
                    TotalBenevolenceBalancedAmount=evnt.StatisticInfo.TotalBenevolenceBalancedAmount,
                    TotalBalancedAmount = evnt.StatisticInfo.TotalBalancedAmount,
                    BalancedDate=evnt.StatisticInfo.BalancedDate,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PartnerTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(PartnerUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Mobile = evnt.Info.Mobile,
                    Region = evnt.Info.Region,
                    Level = (int)evnt.Info.Level,
                    Persent=evnt.Info.Persent,
                    CashPersent = evnt.Info.CashPersent,
                    BalanceInterval=evnt.Info.BalanceInterval,
                    Remark=evnt.Info.Remark,
                    IsLocked=evnt.Info.IsLocked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PartnerTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(PartnerStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    LastBalancedAmount = evnt.StatisticInfo.LastBalancedAmount,
                    LastCashBalancedAmount=evnt.StatisticInfo.LastCashBalancedAmount,
                    LastBenevolenceBalancedAmount=evnt.StatisticInfo.LastBenevolenceBalancedAmount,
                    TotalBalancedAmount = evnt.StatisticInfo.TotalBalancedAmount,
                    TotalCashBalancedAmount=evnt.StatisticInfo.TotalCashBalancedAmount,
                    TotalBenevolenceBalancedAmount=evnt.StatisticInfo.TotalBenevolenceBalancedAmount,
                    BalancedDate = evnt.StatisticInfo.BalancedDate,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PartnerTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(PartnerDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PartnerTable, transaction);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NewBalanceLogEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.LogInfo;
                return connection.InsertAsync(new
                {
                    Id = GuidUtil.NewSequentialId(),
                    PartnerId = evnt.AggregateRootId,
                    WalletId = info.WalletId,
                    Region = info.Region,
                    Amount = info.Amount,
                    BalanceAmount = info.BalanceAmount,
                    CashAmount = info.CashAmount,
                    BenevolenceAmount = info.BenevolenceAmount,
                    CreatedOn = evnt.Timestamp
                }, ConfigSettings.PartnerBalanceLogTable);
            });
        }
    }
}
