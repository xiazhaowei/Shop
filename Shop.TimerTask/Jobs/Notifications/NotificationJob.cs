using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Notifications;
using Shop.QueryServices;
using Shop.TimerTask.Utils;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Shop.TimerTask.Jobs.Notifications
{
    /// <summary>
    /// 预订单任务
    /// </summary>
    public class NotificationJob : IJob
    {
        private ICommandService _commandService;//C端
        private INotificationQueryService _notificationQueryService;//Q 端
        private ISMSender _smSender;//短信发送服务
        private ITemplateMessageSender _templateMessageSender;//微信模板消息发送服务

        public NotificationJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _smSender = container.Resolve<ISMSender>();
            _templateMessageSender = container.Resolve<ITemplateMessageSender>();
            _commandService = container.Resolve<ICommandService>();
            _notificationQueryService = container.Resolve<INotificationQueryService>();
        }
        
        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public  void Execute(IJobExecutionContext context)
        {
            PushNotification();
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        private void PushNotification()
        {
            //获取所有待发送的通知，定时拉取方式
            var notifications = _notificationQueryService.Notifications().Where(x=>!x.IsSmsed || !x.IsMessaged);
            if (notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    if (!notification.IsSmsed)
                    {//发送短信
                        SendSm(notification);
                    }
                    if(!notification.IsMessaged)
                    {//发送微信模板消息
                        SendTemplateMsg(notification);
                    }
                    //设置已发送
                    var command = new SetNotificationSmsedCommand {
                        AggregateRootId= notification.Id
                    };
                    _commandService.SendAsync(command);
                }
            }
        }

        //发送短信
        private void SendSm(QueryServices.Dtos.Notification notification)
        {
            if (notification.Type == Common.Enums.NotificationType.NewStoreOrder)
            {
                //新订单短信
                _smSender.SendMsgNewOrder(notification.Mobile, notification.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (notification.Type == Common.Enums.NotificationType.PayPasswordReseted)
            {
                //新支付密码
                _smSender.SendMsgResetPayPassword(notification.Mobile);
            }
        }
        //发送模板消息
        private void SendTemplateMsg(QueryServices.Dtos.Notification notification)
        {
            if (String.IsNullOrEmpty(notification.WeixinId))
            {
                return;
            }
            try
            {
                //新订单提醒
                if (notification.Type == Common.Enums.NotificationType.NewStoreOrder)
                {
                    dynamic storeOrderInfo = Newtonsoft.Json.Linq.JToken.Parse(notification.AboutObjectStream) as dynamic;
                    _templateMessageSender.SendNewOrderTemplateMessage(notification.WeixinId,
                        storeOrderInfo.Number.ToString(), "商品名称", "用户名称", "登录商城查看", storeOrderInfo.Region.ToString(), "");
                }
                //新支付密码
                if (notification.Type == Common.Enums.NotificationType.PayPasswordReseted)
                {
                    dynamic jsonUserInfo = Newtonsoft.Json.Linq.JToken.Parse(notification.AboutObjectStream) as dynamic;
                    _templateMessageSender.SendChangePayPasswordTemplateMessage(notification.WeixinId,
                        jsonUserInfo.NickName.ToString(), "***", "五福天下商城", "");
                }
                //代理分红到账
                if (notification.Type == Common.Enums.NotificationType.PartnerBalance)
                {
                    dynamic partnerInfo = Newtonsoft.Json.Linq.JToken.Parse(notification.AboutObjectStream) as dynamic;
                    _templateMessageSender.SendMoneyToWalletTemplateMessage(notification.WeixinId,
                        notification.CreatedOn.ToString(), "登录商城查看", "登录商城查看", partnerInfo.Region.ToString() + " 代理分红", "");
                }
            }
            catch { }
        }
    }
}