using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.OfflineStores;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class OfflineStoreViewModelGenerator:BaseGenerator,
        IMessageHandler<OfflineStoreCreatedEvent>,
        IMessageHandler<OfflineStoreUpdatedEvent>,
        IMessageHandler<OfflineStoreDeletedEvent>,
        IMessageHandler<NewSaleAcceptedEvent>,
        IMessageHandler<NewSaleLogEvent>,
        IMessageHandler<OfflineStoreStatisticInfoChangedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(OfflineStoreCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    Name = info.Name,
                    Thumb=info.Thumb,
                    Phone=info.Phone,
                    Description = info.Description,
                    Labels=info.Labels.ExpandAndToString("|"),
                    Region = info.Region,
                    Address = info.Address,
                    Persent=info.Persent,
                    Longitude=info.Longitude,
                    Latitude=info.Latitude,
                    TodaySale = 0,
                    TotalSale = 0,
                    UpdatedOn = evnt.Timestamp,
                    CreatedOn = evnt.Timestamp,
                    IsLocked = 0,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.OfflineStoreTable);
            });
        }
        
        /// <summary>
        /// 更新店铺信息
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(OfflineStoreUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.UpdateAsync(new
                {
                    Name = info.Name,
                    Thumb=info.Thumb,
                    Phone=info.Phone,
                    Description = info.Description,
                    Labels = info.Labels.ExpandAndToString("|"),
                    Region =info.Region,
                    Address = info.Address,
                    Persent=info.Persent,
                    Longitude=info.Longitude,
                    Latitude=info.Latitude,
                    IsLocked=info.IsLocked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OfflineStoreTable);
            });
        }
        
        public Task<AsyncTaskResult> HandleAsync(OfflineStoreStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    TodaySale = evnt.Info.TodaySale,
                    TotalSale = evnt.Info.TotalSale,
                    UpdatedOn = evnt.Info.UpdatedOn,

                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OfflineStoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NewSaleAcceptedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    TodaySale = evnt.Info.TodaySale,
                    TotalSale = evnt.Info.TotalSale,
                    UpdatedOn = evnt.Info.UpdatedOn,

                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OfflineStoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(OfflineStoreDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OfflineStoreTable, transaction);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NewSaleLogEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.SaleLogInfo;
                return connection.InsertAsync(new
                {
                    Id = GuidUtil.NewSequentialId(),
                    OfflineStoreId = evnt.AggregateRootId,
                    UserWalletId = info.UserWalletId,
                    StoreOwnerWalletId = info.StoreOwnerWalletId,
                    StoreName = info.StoreName,
                    Region = info.Region,
                    Address = info.Address,
                    Amount = info.Amount,
                    StoreAmount = info.StoreAmount,
                    UserBenevolence = info.UserBenevolence,
                    CreatedOn = evnt.Timestamp
                }, ConfigSettings.OfflineStoreSaleLogTable);
            });
        }
    }
}
