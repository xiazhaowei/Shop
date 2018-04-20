using ECommon.Dapper;
using ECommon.IO;
using ENode.Infrastructure;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Denormalizers.Dapper
{
    public class PaymentViewModelGenerator : BaseGenerator,
        IMessageHandler<PaymentInitiatedEvent>,
        IMessageHandler<PaymentCompletedEvent>,
        IMessageHandler<PaymentRejectedEvent>
    {
        public Task<AsyncTaskResult> HandleAsync(PaymentInitiatedEvent evnt)
        {
            return TryTransactionAsync((connection, transaction) =>
            {
                var tasks = new List<Task>();
                tasks.Add(connection.InsertAsync(new
                {
                    Id = evnt.AggregateRootId,
                    OrderId = evnt.OrderId,
                    State = (int)PaymentState.Initiated,
                    Description = evnt.Description,
                    TotalAmount = evnt.TotalAmount,
                    ShopCashAmount=0,
                    Version = evnt.Version
                }, ConfigSettings.PaymentTable, transaction));
                foreach (var item in evnt.Items)
                {
                    tasks.Add(connection.InsertAsync(new
                    {
                        Id = item.Id,
                        PaymentId = evnt.AggregateRootId,
                        Description = item.Description,
                        Amount = item.Amount
                    }, ConfigSettings.PaymentItemTable, transaction));
                }
                return tasks;
            });
        }
        public Task<AsyncTaskResult> HandleAsync(PaymentCompletedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    ShopCashAmount=evnt.PayInfo.ShopCash,
                    State = (int)PaymentState.Completed,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PaymentTable);
            });
        }
        public Task<AsyncTaskResult> HandleAsync(PaymentRejectedEvent evnt)
        {
            return TryUpdateRecordAsync(connection =>
            {
                return connection.UpdateAsync(new
                {
                    State = (int)PaymentState.Rejected,
                    Version = evnt.Version
                }, new
                {
                    Id = evnt.AggregateRootId,
                    //Version = evnt.Version - 1
                }, ConfigSettings.PaymentTable);
            });
        }
        
    }
}
