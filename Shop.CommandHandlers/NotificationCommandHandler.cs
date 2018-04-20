using ENode.Commanding;
using Newtonsoft.Json;
using Shop.Commands.Notifications;
using Shop.Common.Enums;
using Shop.Domain.Models.Notifications;
using Shop.Domain.Models.Partners;
using Shop.Domain.Models.Stores;
using Shop.Domain.Models.Users;
using Shop.Domain.Models.Wallets;
using Xia.Common.Extensions;

namespace Shop.CommandHandlers
{
    public class NotificationCommandHandler :
        ICommandHandler<CreateNotificationCommand>,
        ICommandHandler<SetNotificationReadedCommand>,
        ICommandHandler<SetNotificationSmsedCommand>,
        ICommandHandler<DeleteNotificationCommand>
    {
        public void Handle(ICommandContext context, CreateNotificationCommand command)
        {
            var notificationInfo = new NotificationInfo(
                    command.UserId,
                    command.Mobile,
                    command.WeixinId,
                    command.Title,
                    command.Body,
                    command.Type,
                    command.AboutId,
                    command.Remark,
                    command.IsSmsed,
                    command.IsMessaged,
                    command.IsRead,"");
            if (command.Type == NotificationType.NewStoreOrder)
            {
                //店主通知,UserId=StoreId
                var userId = context.Get<Store>(command.UserId).GetUserId();
                var userinfo = context.Get<User>(userId).GetInfo();
                var storeOrderInfo = context.Get<StoreOrder>(command.AboutId).GetInfo();

                notificationInfo.UserId = userId;
                notificationInfo.Mobile = userinfo.Mobile;
                notificationInfo.WeixinId = userinfo.UnionId;
                notificationInfo.Body = "您有新的订单，订单号：{0}，请及时处理，恭祝商祺~".FormatWith(storeOrderInfo.Number);
                notificationInfo.AboutObjectStream = JsonConvert.SerializeObject(storeOrderInfo);
            }
            else if (command.Type==NotificationType.StoreOrderDelivered || command.Type == NotificationType.StoreOrderRefunded || command.Type==NotificationType.StoreOrderAgreeReturn)
            {
                //用户通知 UserId=StoreOrderId
                var storeOrderInfo = context.Get<StoreOrder>(command.UserId).GetInfo();
                var userId = storeOrderInfo.UserId;
                var userinfo = context.Get<User>(userId).GetInfo();

                notificationInfo.UserId = userId;
                notificationInfo.Mobile = userinfo.Mobile;
                notificationInfo.WeixinId = userinfo.UnionId;
                notificationInfo.Body = "您的订单，订单号：{0}".FormatWith(storeOrderInfo.Number);
                notificationInfo.AboutObjectStream = JsonConvert.SerializeObject(storeOrderInfo);
            }
           
            else if(command.Type==NotificationType.StoreOrderConfirmDelivered)
            {
                //店主通知 UserId=StoreOwnerWalletId
                var userId = context.Get<Wallet>(command.UserId).GetOwnerId();
                var userinfo = context.Get<User>(userId).GetInfo();
                var storeOrderInfo = context.Get<StoreOrder>(command.AboutId).GetInfo();

                notificationInfo.UserId = userId;
                notificationInfo.Mobile = userinfo.Mobile;
                notificationInfo.WeixinId = userinfo.UnionId;
                notificationInfo.Body = "您的订单{0}，用户已确认收货，供货金额已结算到您账户".FormatWith(storeOrderInfo.Number);
                notificationInfo.AboutObjectStream = JsonConvert.SerializeObject(storeOrderInfo);
            }
            else if(command.Type==NotificationType.PayPasswordReseted)
            {
                //用户通知 UserId=WalletId
                var userId = context.Get<Wallet>(command.UserId).GetOwnerId();
                var userinfo = context.Get<User>(userId).GetInfo();

                notificationInfo.UserId = userId;
                notificationInfo.Mobile = userinfo.Mobile;
                notificationInfo.WeixinId = userinfo.UnionId;
                notificationInfo.Body = "您的支付密码已重置，请确认是否您本人操作，避免影响您的使用";
                notificationInfo.AboutObjectStream = JsonConvert.SerializeObject(userinfo);
            }
            else if(command.Type==NotificationType.PartnerBalance)
            {
                //用户通知 UserId=PartnerId
                var userId = context.Get<Partner>(command.UserId).GetUserId();
                var userinfo = context.Get<User>(userId).GetInfo();
                var partnerInfo = context.Get<Partner>(command.UserId).GetInfo();

                notificationInfo.UserId = userId;
                notificationInfo.Mobile = userinfo.Mobile;
                notificationInfo.WeixinId = userinfo.UnionId;
                notificationInfo.Body = "恭喜，您代理地区{0}，已成功分红".FormatWith(partnerInfo.Region);
                notificationInfo.AboutObjectStream = JsonConvert.SerializeObject(partnerInfo);
            }
            
            var notification = new Notification(command.AggregateRootId, notificationInfo);

            context.Add(notification);
        }

        public void Handle(ICommandContext context, SetNotificationReadedCommand command)
        {
            context.Get<Notification>(command.AggregateRootId).SetReaded();
        }

        public void Handle(ICommandContext context, SetNotificationSmsedCommand command)
        {
            context.Get<Notification>(command.AggregateRootId).SetSmsed();
        }

        public void Handle(ICommandContext context, DeleteNotificationCommand command)
        {
            context.Get<Notification>(command.AggregateRootId).Delete();
        }
    }
}
