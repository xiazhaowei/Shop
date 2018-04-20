using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Categorys;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class CategoryViewModelGenerator:BaseGenerator,
        IMessageHandler<CategoryCreatedEvent>,
        IMessageHandler<CategoryUpdatedEvent>,
        IMessageHandler<CategoryDeletedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(CategoryCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    ParentId = evnt.ParentId,
                    Name = evnt.Name,
                    Thumb=evnt.Thumb,
                    Type=(int)evnt.Type,
                    Url = evnt.Url,
                    IsShow=evnt.IsShow,
                    Sort=evnt.Sort,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.CategoryTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CategoryUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Name,
                    Url = evnt.Url,
                    Thumb=evnt.Thumb,
                    Type = (int)evnt.Type,
                    IsShow=evnt.IsShow,
                    Sort =evnt.Sort,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.CategoryTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(CategoryDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.CategoryTable, transaction);
            });
        }
    }
}
