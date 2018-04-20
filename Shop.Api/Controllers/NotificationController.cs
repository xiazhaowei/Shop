using ENode.Commanding;
using Shop.Api.Models.Request;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Notifications;
using Shop.Api.Services;
using Shop.QueryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xia.Common.Extensions;
using Shop.Commands.Notifications;
using Shop.Api.Extensions;
using System.Threading.Tasks;

namespace Shop.Api.Controllers
{
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
        public BaseApiResponse MyNotifications()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

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