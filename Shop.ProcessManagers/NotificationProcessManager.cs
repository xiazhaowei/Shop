using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Notifications;
using Shop.Common.Enums;
using Shop.Domain.Events.Stores.StoreOrders;
using System.Threading.Tasks;
using Xia.Common;
using System;
using Shop.Domain.Events.Wallets;
using Shop.Domain.Events.Partners;

namespace Shop.ProcessManagers
{
    [Component]
    public class NotificationProcessManager :
        IMessageHandler<StoreOrderCreatedEvent>,                    //创建商家订单时
        IMessageHandler<StoreOrderExpressedEvent>,                 //商家订单发货时
        IMessageHandler<StoreOrderConfirmExpressedEvent>,       //用户确认收货，给商家结算
        IMessageHandler<AgreeReturnEvent>,                              //商家同意退货时
        IMessageHandler<AgreeRefundedEvent>,                         //同意包裹退款
        IMessageHandler<WalletAccessCodeUpdatedEvent>,           //设置支付密码
        IMessageHandler<NewBalanceLogEvent>                             //代理分红
    {
        private ICommandService _commandService;

        public NotificationProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        

        /// <summary>
        /// 创建商家订单
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreOrderCreatedEvent evnt)
        {
            return _commandService.SendAsync( CreatedNotificationCommand(
                evnt.Info.StoreId,
                "新订单",
                NotificationType.NewStoreOrder,
                evnt.AggregateRootId,
                true));
        }

        public Task<AsyncTaskResult> HandleAsync(AgreeRefundedEvent evnt)
        {
            return _commandService.SendAsync( CreatedNotificationCommand(
                evnt.AggregateRootId,
                "订单已退款",
                NotificationType.StoreOrderRefunded,
                evnt.AggregateRootId));
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreOrderConfirmExpressedEvent evnt)
        {
            return _commandService.SendAsync(CreatedNotificationCommand(
                evnt.StoreOwnerWalletId,
                "订单确认收货",
                NotificationType.StoreOrderConfirmDelivered,
                evnt.AggregateRootId));
        }

        public Task<AsyncTaskResult> HandleAsync(StoreOrderExpressedEvent evnt)
        {
            return _commandService.SendAsync(CreatedNotificationCommand(
                evnt.AggregateRootId,
                "订单已发货",
                NotificationType.StoreOrderDelivered,
                evnt.AggregateRootId));
        }

        /// <summary>
        /// 同意退货
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(AgreeReturnEvent evnt)
        {
            return _commandService.SendAsync(CreatedNotificationCommand(
                evnt.AggregateRootId,
                "商家同意退货",
                NotificationType.StoreOrderAgreeReturn,
                evnt.AggregateRootId));
        }

        //设置支付密码
        public Task<AsyncTaskResult> HandleAsync(WalletAccessCodeUpdatedEvent evnt)
        {
            return _commandService.SendAsync(CreatedNotificationCommand(
                evnt.AggregateRootId,
                "设置支付密码",
                NotificationType.PayPasswordReseted,
                evnt.AggregateRootId,
                true));
        }

        //代理分红
        public Task<AsyncTaskResult> HandleAsync(NewBalanceLogEvent evnt)
        {
            return _commandService.SendAsync(CreatedNotificationCommand(
                evnt.AggregateRootId,
                "代理已分红",
                NotificationType.PartnerBalance,
                evnt.AggregateRootId,
                true));
        }


        private CreateNotificationCommand CreatedNotificationCommand(
            Guid userId,
            string title,
            NotificationType type,
            Guid aboutId,
            bool isSms=false,
            bool isMessage=true)
        {
            return new CreateNotificationCommand(
                GuidUtil.NewSequentialId(),
                userId,
                "Mobile",
                "WeixinId",//在commandHandler会修改掉
                title,
                "",
                type,
                aboutId,
                "",
                !isSms, //短信通知
                !isMessage,
                false);
        }
    }
}
