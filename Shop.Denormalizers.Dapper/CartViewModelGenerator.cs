using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Carts;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class CartViewModelGenerator : BaseGenerator,
        IMessageHandler<CartCreatedEvent>,
        IMessageHandler<CartAddedGoodsEvent>,
        IMessageHandler<CartRemovedGoodsEvent>,
        IMessageHandler<CartGoodsQuantityChangedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(CartCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId=evnt.UserId,
                    GoodsCount=0,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.CartTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CartAddedGoodsEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    GoodsCount=evnt.FinalCount,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.CartTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = evnt.CartGoodsId,
                        CartId = evnt.AggregateRootId,

                        StoreId = evnt.Info.StoreId,
                        GoodsId = evnt.Info.GoodsId,
                        SpecificationId = evnt.Info.SpecificationId,

                        SpecificationName = evnt.Info.SpecificationName,
                        GoodsName = evnt.Info.GoodsName,
                        GoodsPic=evnt.Info.GoodsPic,
                        Price=evnt.Info.Price,
                        OriginalPrice = evnt.Info.OriginalPrice,
                        Stock=evnt.Info.Stock,
                        Quantity=evnt.Info.Quantity,
                        Benevolence = evnt.Info.Benevolence,
                        CreatedOn = evnt.Timestamp
                    }, ConfigSettings.CartGoodsesTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CartRemovedGoodsEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    GoodsCount = evnt.FinalCount,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.CartTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.DeleteAsync(new
                    {
                        CartId = evnt.AggregateRootId,
                        Id = evnt.CartGoodsId
                    }, ConfigSettings.CartGoodsesTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CartGoodsQuantityChangedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    GoodsCount=evnt.FinalCount,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.CartTable, transaction);

                if (effectedRows == 1)
                {
                    await connection.UpdateAsync(new
                    {
                        Quantity = evnt.FinalQuantity
                    }, new
                    {
                        CartId = evnt.AggregateRootId,
                        Id = evnt.CartGoodsId
                    }, ConfigSettings.CartGoodsesTable, transaction);
                }
            });
        }
    }
}
