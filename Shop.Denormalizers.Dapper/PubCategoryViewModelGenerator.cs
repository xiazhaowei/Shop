using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.PubCategorys;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class PubCategoryViewModelGenerator:BaseGenerator,
        IMessageHandler<PubCategoryCreatedEvent>,
        IMessageHandler<PubCategoryUpdatedEvent>,
        IMessageHandler<PubCategoryDeletedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(PubCategoryCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    ParentId = evnt.ParentId,
                    Name = evnt.Name,
                    Thumb=evnt.Thumb,
                    IsShow=evnt.IsShow,
                    Sort=evnt.Sort,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.PubCategoryTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(PubCategoryUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Name,
                    Thumb = evnt.Thumb,
                    IsShow=evnt.IsShow,
                    Sort = evnt.Sort,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PubCategoryTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(PubCategoryDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PubCategoryTable, transaction);
            });
        }
    }
}
