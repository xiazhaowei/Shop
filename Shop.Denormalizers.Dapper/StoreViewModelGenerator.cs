using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class StoreViewModelGenerator:BaseGenerator,
        IMessageHandler<StoreCreatedEvent>,
        IMessageHandler<StoreCustomerUpdatedEvent>,
        IMessageHandler<StoreUpdatedEvent>,
        IMessageHandler<StoreReturnAddressUpdatedEvent>,
        IMessageHandler<StoreLockedEvent>,
        IMessageHandler<StoreUnLockedEvent>,
        IMessageHandler<StoreStatisticInfoChangedEvent>,

        IMessageHandler<SubjectInfoUpdatedEvent>,
        IMessageHandler<StoreAccessCodeUpdatedEvent>,
        IMessageHandler<StoreStatusUpdatedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(StoreCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.UserId,
                    AccessCode=info.AccessCode,
                    Name = info.Name,
                    Description = info.Description,
                    Region = info.Region,
                    Address = info.Address,
                    SubjectName=evnt.SubjectInfo.SubjectName,
                    SubjectNumber=evnt.SubjectInfo.SubjectNumber,
                    SubjectPic=evnt.SubjectInfo.SubjectPic,
                    UpdatedOn=evnt.Timestamp,

                    TodaySale = 0,
                    TotalSale = 0,
                    TodayOrder = 0,
                    TotalOrder = 0,
                    OnSaleGoodsCount = 0,

                    CreatedOn =evnt.Timestamp,
                    Type=(int)StoreType.ThirdParty,
                    Status = (int)StoreStatus.Apply,
                    IsLocked = 0,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.StoreTable);
            });
        }
        /// <summary>
        /// 商家更新店铺信息
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreCustomerUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.UpdateAsync(new
                {
                    Name = info.Name,
                    Description = info.Description,
                    Address = info.Address,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }
        /// <summary>
        /// 后台更新店铺信息
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.UpdateAsync(new
                {
                    Name = info.Name,
                    Description = info.Description,
                    Address = info.Address,
                    Type=(int)info.Type,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(StoreLockedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsLocked = 1,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(StoreUnLockedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsLocked = 0,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(StoreStatisticInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    TodaySale = evnt.StatisticInfo.TodaySale,
                    TotalSale = evnt.StatisticInfo.TotalSale,
                    TodayOrder=evnt.StatisticInfo.TodayOrder,
                    TotalOrder=evnt.StatisticInfo.TotalOrder,
                    OnSaleGoodsCount = evnt.StatisticInfo.OnSaleGoodsCount,
                    UpdatedOn=evnt.StatisticInfo.UpdatedOn,

                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(SubjectInfoUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                var info = evnt.SubjectInfo;
                return connection.UpdateAsync(new
                {
                    SubjectName = info.SubjectName,
                    SubjectNumber = info.SubjectNumber,
                    SubjectPic = info.SubjectPic,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreAccessCodeUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    AccessCode = evnt.AccessCode,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreStatusUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status =(int)evnt.Status,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreReturnAddressUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.UpdateAsync(new
                {
                    ReturnAddressName = info.Name,
                    ReturnAddress = info.Address,
                    ReturnAddressMobile = info.Mobile,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreTable);
            });
        }
    }
}
