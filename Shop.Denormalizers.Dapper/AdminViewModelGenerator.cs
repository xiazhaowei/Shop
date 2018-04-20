using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Domain.Events.Admins;
using System.Threading.Tasks;
using Xia.Common;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class AdminViewModelGenerator:BaseGenerator,
        IMessageHandler<AdminCreatedEvent>,
        IMessageHandler<AdminUpdatedEvent>,
        IMessageHandler<AdminDeletedEvent>,
        IMessageHandler<AdminPasswordUpdatedEvent>,
        IMessageHandler<NewOperatRecordEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(AdminCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Name = evnt.Info.Name,
                    LoginName=evnt.Info.LoginName,
                    Portrait=evnt.Info.Portrait,
                    Password =evnt.Info.Password,
                    Role=(int)evnt.Info.Role,
                    IsLocked=evnt.Info.IsLocked,
                    CreatedOn = evnt.Timestamp,
                    Version = evnt.Version,
                    EventSequence=evnt.Sequence
                }, ConfigSettings.AdminTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AdminUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Name = evnt.Info.Name,
                    LoginName = evnt.Info.LoginName,
                    Portrait=evnt.Info.Portrait,
                    Role = (int)evnt.Info.Role,
                    IsLocked=evnt.Info.IsLocked,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.AdminTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AdminDeletedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.DeleteAsync(new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.AdminTable, transaction);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AdminPasswordUpdatedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Password = evnt.Password,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.AdminTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(NewOperatRecordEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                //尽管是更新ExpressAddresssTable但是也要更新聚合跟，因为地址属于聚合跟的状态
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.AdminTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id = GuidUtil.NewSequentialId(),
                        AdminId = evnt.AggregateRootId,
                        AboutId = evnt.Info.AboutId,
                        AdminName = evnt.Info.AdminName,
                        Operat = evnt.Info.Operat,
                        Remark = evnt.Info.Remark,
                        CreatedOn = evnt.Timestamp
                    }, ConfigSettings.AdminOperatRecordTable, transaction);
                }
            });
        }
    }
}
