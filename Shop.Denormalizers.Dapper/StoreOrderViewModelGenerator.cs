using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores.StoreOrders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class StoreOrderViewModelGenerator:BaseGenerator,
        IMessageHandler<StoreOrderCreatedEvent>,
        IMessageHandler<StoreOrderDeletedEvent>,
        IMessageHandler<StoreOrderExpressedEvent>,
        IMessageHandler<StoreOrderReturnExpressedEvent>,
        IMessageHandler<StoreOrderConfirmExpressedEvent>,
        IMessageHandler<ApplyRefundedEvent>,
        IMessageHandler<ApplyReturnAndRefundedEvent>,
        IMessageHandler<AgreeReturnEvent>,
        IMessageHandler<AgreeRefundedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(StoreOrderCreatedEvent evnt)
        {
            return TryTransactionAsync((connection, transaction) =>
            {
                var tasks = new List<Task>();

                //插入订单主记录
                tasks.Add(connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId=evnt.Info.UserId,
                    OrderId=evnt.Info.OrderId,
                    StoreId=evnt.Info.StoreId,

                    WalletId=evnt.WalletId,
                    StoreOwnerWalletId = evnt.StoreOwnerWalletId,

                    Region=evnt.Info.Region,
                    Number=evnt.Info.Number,
                    Remark=evnt.Info.Remark,

                    ExpressRegion=evnt.ExpressAddressInfo.Region,
                    ExpressAddress=evnt.ExpressAddressInfo.Address,
                    ExpressName=evnt.ExpressAddressInfo.Name,
                    ExpressMobile=evnt.ExpressAddressInfo.Mobile,
                    ExpressZip=evnt.ExpressAddressInfo.Zip,

                    CreatedOn = evnt.Timestamp,
                    Total = evnt.PayDetailInfo.Total,
                    ShopCash = evnt.PayDetailInfo.ShopCash,
                    StoreTotal=evnt.PayDetailInfo.StoreTotal,
                    Status=(int)StoreOrderStatus.Placed,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.StoreOrderTable, transaction));

                //插入订单明细?这里交给流程处理器处理
                return tasks;
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreOrderExpressedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    DeliverExpressName=evnt.ExpressInfo.ExpressName,
                    DeliverExpressCode=evnt.ExpressInfo.ExpressCode,
                    DeliverExpressNumber=evnt.ExpressInfo.ExpressNumber,
                    DeliverTime=evnt.Timestamp,
                    Status = (int)StoreOrderStatus.Expressing,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(ApplyRefundedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Reason = evnt.RefundApplyInfo.Reason,
                    RefundAmount=evnt.RefundApplyInfo.RefundAmount,
                    ApplyRefundTime=evnt.Timestamp,
                    Status = (int)StoreOrderStatus.OnlyRefund,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(ApplyReturnAndRefundedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Reason = evnt.RefundApplyInfo.Reason,
                    RefundAmount = evnt.RefundApplyInfo.RefundAmount,
                    ApplyRefundTime=evnt.Timestamp,
                    Status = (int)StoreOrderStatus.ReturnAndRefund,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AgreeReturnEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    StoreRemark=evnt.Remark,
                    AgreeReturnTime=evnt.Timestamp,
                    Status = (int)StoreOrderStatus.AgreeReturn,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AgreeRefundedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)StoreOrderStatus.Closed,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreOrderConfirmExpressedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)StoreOrderStatus.Success,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreOrderDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                //删除订单
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable, transaction);

                if (effectedRows == 1)
                {
                    var tasks = new List<Task>();
                    //删除订单商品
                    tasks.Add(connection.DeleteAsync(new
                    {
                        OrderId = evnt.AggregateRootId
                    }, ConfigSettings.OrderGoodsTable, transaction));
                    

                    Task.WaitAll(tasks.ToArray());
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(StoreOrderReturnExpressedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ReturnDeliverExpressName = evnt.ExpressInfo.ExpressName,
                    ReturnDeliverExpressCode = evnt.ExpressInfo.ExpressCode,
                    ReturnDeliverExpressNumber = evnt.ExpressInfo.ExpressNumber,
                    Status = (int)StoreOrderStatus.ReturnExpressing,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.StoreOrderTable);
            });
        }
    }
}
