using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Notifications;
using Shop.Apis.Services;
using Shop.QueryServices;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class NotificationController:BaseApiController
    {
        private INotificationQueryService _notificationQueryService;

        public NotificationController(ICommandService commandService, IContextService contextService,
            INotificationQueryService notificationQueryService):base(commandService,contextService)
        {
            _notificationQueryService = notificationQueryService;
        }

        /// <summary>
        /// 我的通知 只现在最近的20个通知即可
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MyNotifications")]
        public BaseApiResponse MyNotifications()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var notifications = _notificationQueryService.UserNotifications(currentAccount.UserId.ToGuid()).OrderByDescending(x=>x.CreatedOn).Take(20);
            return new MyNotificationsResponse
            {
                Notifications = notifications.Select(x => new Notification
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Mobile = x.Mobile,
                    Title=x.Title,
                    Body=x.Body,
                    Type=x.Type,
                    CreatedOn = x.CreatedOn.GetTimeSpan(),
                    AboutId=x.AboutId,
                    Remark=x.Remark,
                    IsRead=x.IsRead
                }).ToList()
            };
        }
        
    }
}