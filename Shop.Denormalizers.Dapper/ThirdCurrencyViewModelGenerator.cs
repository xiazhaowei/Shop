using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.ThirdCurrencys;
using System.Threading.Tasks;
using Xia.Common;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class ThirdCurrencyViewModelGenerator:BaseGenerator,
        IMessageHandler<ThirdCurrencyCreatedEvent>,
        IMessageHandler<ThirdCurrencyUpdatedEvent>,
        IMessageHandler<ThirdCurrencyDeletedEvent>,
        IMessageHandler<ThirdCurrencyImportedAmountChangedEvent>,
        IMessageHandler<ThirdCurrencyMaxImportAmountChangedEvent>,
        IMessageHandler<NewThirdCurrencyImportLogEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(ThirdCurrencyCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name=evnt.Info.Name,
                    Icon = evnt.Info.Icon,
                    CompanyName = evnt.Info.CompanyName,
                    Conversion = evnt.Info.Conversion,
                    ImportedAmount=0,
                    MaxImportAmount=0,
                    CreatedOn = evnt.Timestamp,
                    Remark=evnt.Info.Remark,
                    IsLocked=evnt.Info.IsLocked,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.ThirdCurrencyTable);
            });
        }
        
        public Task<AsyncTaskResult> HandleAsync(ThirdCurrencyUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Info.Name,
                    Icon = evnt.Info.Icon,
                    CompanyName = evnt.Info.CompanyName,
                    Conversion = evnt.Info.Conversion,
                    Remark=evnt.Info.Remark,
                    IsLocked=evnt.Info.IsLocked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.ThirdCurrencyTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(ThirdCurrencyDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.ThirdCurrencyTable, transaction);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NewThirdCurrencyImportLogEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.ThirdCurrencyTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        ThirdCurrencyId = evnt.AggregateRootId,
                        WalletId = evnt.LogInfo.WalletId,
                        Mobile = evnt.LogInfo.Mobile,
                        Account = evnt.LogInfo.Account,
                        Amount = evnt.LogInfo.Amount,
                        ShopCashAmount=evnt.LogInfo.ShopCashAmount,
                        Conversion=evnt.LogInfo.Conversion,
                        CreatedOn=evnt.Timestamp
                    }, ConfigSettings.ThirdCurrencyImportLogTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(ThirdCurrencyImportedAmountChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ImportedAmount = evnt.ImportedAmount,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.ThirdCurrencyTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(ThirdCurrencyMaxImportAmountChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    MaxImportAmount = evnt.FinallyAmount,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.ThirdCurrencyTable);
            });
        }
    }
}
