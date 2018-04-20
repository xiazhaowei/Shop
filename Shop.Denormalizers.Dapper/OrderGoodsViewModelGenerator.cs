using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores.StoreOrders;
using Shop.Domain.Events.Stores.StoreOrders.GoodsServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class OrderGoodsViewModelGenerator:BaseGenerator,
        IMessageHandler<OrderGoodsCreatedEvent>,
        IMessageHandler<ServiceApplyedEvent>,
        IMessageHandler<ServiceAgreedEvent>,
        IMessageHandler<ServiceExpressedEvent>,
        IMessageHandler<AgreedRefundEvent>,
        IMessageHandler<DisAgreedRefundEvent>,
        IMessageHandler<ServiceFinishedEvent>,
        IMessageHandler<ServiceExpiredEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(OrderGoodsCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                var info = evnt.Info;
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    OrderId = evnt.OrderId,
                    GoodsId = info.GoodsId,
                    SpecificationId = info.SpecificationId,

                    WalletId=info.WalletId,
                    StoreOwnerWalletId=info.StoreOwnerWalletId,

                    GoodsName=info.GoodsName,
                    GoodsPic=info.GoodsPic,
                    SpecificationName = info.SpecificationName,
                    Quantity=info.Quantity,
                    Price= info.Price,
                    OriginalPrice = info.OriginalPrice,
                    Total=info.PayDetailInfo.Total,
                    ShopCash=info.PayDetailInfo.ShopCash,
                    StoreTotal=info.PayDetailInfo.StoreTotal,
                    Benevolence = info.Benevolence,
                    CreatedOn=evnt.Timestamp,
                    ServiceExpirationDate=evnt.ServiceExpirationDate,
                    Status=(int)OrderGoodsStatus.Normal,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.OrderGoodsTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(ServiceApplyedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var info = evnt.Info;
                var effectedRows = await connection.UpdateAsync(new
                {
                    Status=(int)info.ServiceType,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderGoodsTable, transaction);

                if (effectedRows == 1)
                {//聚合跟状态已经更新
                    var tasks = new List<Task>();

                    //插入申请记录
                    tasks.Add(connection.InsertAsync(new
                    {
                        OrderGoodsId = evnt.AggregateRootId,
                        ServiceNumber = info.ServiceNumber,
                        Quantity = info.Quantity,
                        Reason = info.Reason,
                        Remark = info.Remark,
                        CreatedOn = evnt.Timestamp
                    }, ConfigSettings.ApplyServiceTable));

                    await Task.WhenAll(tasks);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(ServiceAgreedEvent evnt)
        {
                return TryUpdateRecordAsync(connection=> {
                    return connection.UpdateAsync(new
                    {
                        Status = (int)evnt.Status,
                        Version = evnt.Version,
                        EventSequence = evnt.Sequence
                    }, new
                    {
                        Id = evnt.AggregateRootId,
                        //Version = evnt.Version - 1
                    }, ConfigSettings.OrderGoodsTable);
                });
        }
        public Task<AsyncTaskResult> HandleAsync(ServiceExpressedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var info = evnt.Info;
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderGoodsTable, transaction);

                if (effectedRows == 1)
                {//聚合跟状态已经更新
                    var tasks = new List<Task>();

                    //插入申请记录
                    tasks.Add(connection.InsertAsync(new
                    {
                        OrderGoodsId = evnt.AggregateRootId,
                        ServiceNumber = info.ServiceNumber,
                        ExpressName = info.ExpressName,
                        ExpressNumber = info.ExpressNumber,
                        CreatedOn = evnt.Timestamp,
                    }, ConfigSettings.ServiceExpressTable));

                    await Task.WhenAll(tasks);
                }
            });
        }
        public Task<AsyncTaskResult> HandleAsync(AgreedRefundEvent evnt)
        {
                return TryUpdateRecordAsync(connection=> {
                    return connection.UpdateAsync(new
                    {
                        Status = (int)OrderGoodsStatus.Closed,
                        Version = evnt.Version,
                        EventSequence = evnt.Sequence
                    }, new
                    {
                        Id = evnt.AggregateRootId,
                        //Version = evnt.Version - 1
                    }, ConfigSettings.OrderGoodsTable);
                });
        }
        public Task<AsyncTaskResult> HandleAsync(DisAgreedRefundEvent evnt)
        {
            return TryUpdateRecordAsync(connection => {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderGoodsStatus.Closed,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderGoodsTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(ServiceFinishedEvent evnt)
        {
            return TryUpdateRecordAsync(connection => {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderGoodsStatus.Closed,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderGoodsTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(ServiceExpiredEvent evnt)
        {
            return TryUpdateRecordAsync(connection => {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderGoodsStatus.Expire,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderGoodsTable);
            });
        }
    }
}
