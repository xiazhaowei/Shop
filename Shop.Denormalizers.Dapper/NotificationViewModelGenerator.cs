using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Notifications;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class NotificationViewModelGenerator:BaseGenerator,
        IMessageHandler<NotificationCreatedEvent>,
        IMessageHandler<NotificationSmsedEvent>,
        IMessageHandler<NotificationReadedEvent>,
        IMessageHandler<NotificationDeletedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(NotificationCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    UserId = evnt.Info.UserId,
                    Mobile=evnt.Info.Mobile,
                    WeixinId=evnt.Info.WeixinId,
                    Title = evnt.Info.Title,
                    Body = evnt.Info.Body,
                    Type = (int)evnt.Info.Type,
                    AboutId = evnt.Info.AboutId,
                    Remark = evnt.Info.Remark,
                    IsRead = evnt.Info.IsRead,
                    IsSmsed = evnt.Info.IsSmsed,
                    IsMessaged = evnt.Info.IsMessaged,
                    AboutObjectStream=evnt.Info.AboutObjectStream,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.NotificationTable);
            });
        }
        

        public Task<AsyncTaskResult> HandleAsync(NotificationSmsedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsSmsed=1,
                    IsMessaged = 1,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.NotificationTable);
            });
        }


        public Task<AsyncTaskResult> HandleAsync(NotificationDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.NotificationTable, transaction);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NotificationReadedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    IsRead = 1,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.NotificationTable);
            });
        }
    }
}
