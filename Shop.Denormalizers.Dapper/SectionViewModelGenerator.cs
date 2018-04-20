using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Stores.Sections;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class SectionViewModelGenerator:BaseGenerator,
       IMessageHandler<SectionCreatedEvent>,
        IMessageHandler<SectionChangedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(SectionCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Name,
                    CreatedOn = evnt.Timestamp,
                    Description = evnt.Description,
                    Version = evnt.Version,
                    EvntSequence=evnt.Sequence
                }, ConfigSettings.SectionTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(SectionChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Name,
                    Description = evnt.Description,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.SectionTable);
            });
        }
    }
}
