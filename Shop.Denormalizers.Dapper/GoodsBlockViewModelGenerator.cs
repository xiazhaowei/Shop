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
    public class GoodsBlockViewModelGenerator:BaseGenerator,
        IMessageHandler<GoodsBlockCreatedEvent>,
        IMessageHandler<GoodsBlockUpdatedEvent>,
        IMessageHandler<GoodsBlockDeletedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(GoodsBlockCreatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows=await connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Info.Name,
                    Thumb=evnt.Info.Thumb,
                    Banner = evnt.Info.Banner,
                    Layout=(int)evnt.Info.Layout,
                    IsShow=evnt.Info.IsShow,
                    Sort=evnt.Info.Sort,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.GoodsBlockTable,transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockGoodsesTable, transaction));

                //插入新的记录
                foreach (var goodsId in evnt.Info.Goodses)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsBlockId = evnt.AggregateRootId,
                        GoodsId = goodsId
                    }, ConfigSettings.GoodsBlockGoodsesTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsBlockUpdatedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows= await connection.UpdateAsync(new
                {
                    Name = evnt.Info.Name,
                    Thumb = evnt.Info.Thumb,
                    Banner=evnt.Info.Banner,
                    Layout=(int)evnt.Info.Layout,
                    IsShow=evnt.Info.IsShow,
                    Sort =evnt.Info.Sort,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsBlockTable, transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockGoodsesTable, transaction));

                //插入新的记录
                foreach (var goodsId in evnt.Info.Goodses)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        GoodsBlockId = evnt.AggregateRootId,
                        GoodsId = goodsId
                    }, ConfigSettings.GoodsBlockGoodsesTable, transaction));
                }
                Task.WaitAll(tasks.ToArray());

            });
        }

        public Task<AsyncTaskResult> HandleAsync(GoodsBlockDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GoodsBlockTable, transaction);

                var tasks = new List<Task>();
                //删除原来的记录
                tasks.Add(connection.DeleteAsync(new
                {
                    GoodsBlockId = evnt.AggregateRootId
                }, ConfigSettings.GoodsBlockGoodsesTable, transaction));

                Task.WaitAll(tasks.ToArray());
            });
        }
    }
}
