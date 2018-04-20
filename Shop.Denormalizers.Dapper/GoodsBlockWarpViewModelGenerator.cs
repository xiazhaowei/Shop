using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.GoodsBlocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class GoodsBlockWarpViewModelGenerator:BaseGenerator,
        IMessageHandler<GoodsBlockWarpCreatedEvent>,
        IMessageHandler<GoodsBlockWarpUpdatedEvent>,
        IMessageHandler<GoodsBlockWarpDeletedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(GoodsBlockWarpCreatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows=await connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name=evnt.Info.Name,
                    Style=(int)evnt.Info.Style,
                    IsShow=evnt.Info.IsShow,
                    Sort=evnt.Info.Sort,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.GoodsBlockWarpTable,transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable, transaction));

                //插入新的记录
                foreach (var goodsBlockId in evnt.Info.GoodsBlocks)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsBlockWarpId = evnt.AggregateRootId,
                        GoodsBlockId = goodsBlockId
                    }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsBlockWarpUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows= await connection.UpdateAsync(new
                {
                    Name=evnt.Info.Name,
                    Style=(int)evnt.Info.Style,
                    IsShow=evnt.Info.IsShow,
                    Sort =evnt.Info.Sort,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsBlockWarpTable, transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockWarpId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable, transaction));

                //插入新的记录
                foreach (var goodsBlockId in evnt.Info.GoodsBlocks)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsBlockWarpId = evnt.AggregateRootId,
                        GoodsBlockId = goodsBlockId
                    }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());

            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsBlockWarpDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsBlockWarpTable, transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockWarpId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable, transaction));

                Task.WaitAll(tasks.ToArray());
            });
        }
    }
}
