using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Grantees;
using System;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Denormalizers.Dapper
{
    /// <summary>
    /// 更新读库 Dapper
    /// </summary>
    public class GranteeViewModelGenerator : BaseGenerator,
        IMessageHandler<GranteeCreatedEvent>,
        IMessageHandler<AcceptedNewMoneyHelpEvent>,
        IMessageHandler<AddTestifyedEvent>,
        IMessageHandler<GranteeStatisticsInfoChangedEvent>,
        IMessageHandler<GranteeVerifyedEvent>,
        IMessageHandler<GranteeUnVerifyedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(GranteeCreatedEvent evnt)
        {
            return TryInsertRecordAsync(connection =>
            {
                return connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    Publisher = evnt.Publisher,
                    Title = evnt.Info.Title,
                    Descriptions = evnt.Info.Description,
                    Pics=evnt.Info.Pics.ExpandAndToString("|"),
                    Max=evnt.Info.Max,
                    Days=evnt.Info.Days,
                    ExpiredOn=evnt.Info.ExpiredOn,
                    HelpCount=0,
                    Goods=0,
                    Total=0,
                    CreatedOn = evnt.Timestamp,
                    Status = (int)GranteeStatus.Placed,
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, ConfigSettings.GranteeTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AcceptedNewMoneyHelpEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GranteeTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        GranteeId = evnt.AggregateRootId,
                        UserId = evnt.MoneyHelp.UserId,
                        Amount = evnt.MoneyHelp.Amount,
                        CreatedOn = evnt.Timestamp,
                        Says = evnt.MoneyHelp.Says
                    }, ConfigSettings.GranteeMoneyHelpsTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(AddTestifyedEvent evnt)
        {
            return TryTransactionAsync(async (connection, transaction) =>
            {
                var effectedRows = await connection.UpdateAsync(new
                {
                    Version = evnt.Version,
                    EventSequence = evnt.Sequence
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GranteeTable, transaction);
                if (effectedRows == 1)
                {
                    await connection.InsertAsync(new
                    {
                        Id=Guid.NewGuid(),
                        GranteeId = evnt.AggregateRootId,
                        UserId = evnt.Testify.UserId,
                        Name = evnt.Testify.Name,
                        Relationship=evnt.Testify.Relationship,
                        Mobile=evnt.Testify.Mobile,
                        Remark=evnt.Testify.Remark,
                        CreatedOn = evnt.Timestamp
                    }, ConfigSettings.GranteeTestifysTable, transaction);
                }
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GranteeStatisticsInfoChangedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    HelpCount = evnt.StatisticsInfo.Count,
                    Goods = evnt.StatisticsInfo.Goods,
                    Total = evnt.StatisticsInfo.Total,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GranteeTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GranteeVerifyedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status=(int)GranteeStatus.Verifyed,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GranteeTable);
            });
        }

        public Task<AsyncTaskResult> HandleAsync(GranteeUnVerifyedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    Status = (int)GranteeStatus.Placed,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.GranteeTable);
            });
        }
    }
}
