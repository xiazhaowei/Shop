using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    public class OrderViewModelGenerator : BaseGenerator,
        IMessageHandler<OrderPlacedEvent>,
        IMessageHandler<OrderReservationConfirmedEvent>,
        IMessageHandler<OrderPaymentConfirmedEvent>,
        IMessageHandler<OrderExpiredEvent>,
        IMessageHandler<OrderClosedEvent>,
        IMessageHandler<OrderSuccessedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(OrderPlacedEvent evnt)
        {
            return TryTransactionAsync((connection, transaction) =>
            {
                var tasks = new List<Task>();

                //插入订单主记录
                tasks.Add(connection.InsertAsync(new
                {
                    OrderId = evnt.AggregateRootId,
                    UserId=evnt.UserId,
                    ExpressRegion = evnt.ExpressAddressInfo.Region,
                    ExpressAddress = evnt.ExpressAddressInfo.Address,
                    ExpressName = evnt.ExpressAddressInfo.Name,
                    ExpressMobile = evnt.ExpressAddressInfo.Mobile,
                    ExpressZip = evnt.ExpressAddressInfo.Zip,
                    Status = (int)OrderStatus.Placed,
                    ReservationExpirationDate = evnt.ReservationExpirationDate,
                    Total = evnt.OrderTotal.Total,
                    ShopCash = 0,
                    StoreTotal = evnt.OrderTotal.StoreTotal,
                    Version = evnt.Version
                }, ConfigSettings.OrderTable, transaction));

                //插入订单明细
                foreach (var line in evnt.OrderTotal.Lines)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        OrderId = evnt.AggregateRootId,
                        GoodsId=line.SpecificationQuantity.Specification.GoodsId,
                        StoreId=line.SpecificationQuantity.Specification.StoreId,
                        SpecificationId = line.SpecificationQuantity.Specification.SpecificationId,
                        GoodsName = line.SpecificationQuantity.Specification.GoodsName,
                        GoodsPic=line.SpecificationQuantity.Specification.GoodsPic,
                        SpecificationName = line.SpecificationQuantity.Specification.SpecificationName,
                        Quantity = line.SpecificationQuantity.Quantity,
                        Price = line.SpecificationQuantity.Specification.Price,
                        OriginalPrice = line.SpecificationQuantity.Specification.OriginalPrice,
                        Benevolence=line.SpecificationQuantity.Specification.Benevolence,
                        LineTotal = line.LineTotal,
                        StoreLineTotal = line.StoreLineTotal
                    }, ConfigSettings.OrderLineTable, transaction));
                }
                return tasks;
            });
        }
        
        public Task<AsyncTaskResult> HandleAsync(OrderReservationConfirmedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)evnt.OrderStatus,
                    Version = evnt.Version
                }, new
                {
                    OrderId = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(OrderPaymentConfirmedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ShopCash=evnt.PayInfo.ShopCash,
                    Status = (int)evnt.OrderStatus,
                    Version = evnt.Version
                }, new
                {
                    OrderId = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(OrderExpiredEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderStatus.Expired,
                    Version = evnt.Version
                }, new
                {
                    OrderId = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(OrderClosedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderStatus.Closed,
                    Version = evnt.Version
                }, new
                {
                    OrderId = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(OrderSuccessedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)OrderStatus.Success,
                    Version = evnt.Version
                }, new
                {
                    OrderId = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.OrderTable);
            });
        }

    }
}
