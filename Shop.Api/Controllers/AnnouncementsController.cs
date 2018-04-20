using ENode.Commanding;
using Shop.Api.Models.Request.Announcements;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Announcements;
using Shop.Api.Services;
using Shop.QueryServices;
using System.Linq;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    /// <summary>
    /// 公告
    /// </summary>
    public class AnnouncementController : BaseApiController
    {
        private IAnnouncementQueryService _announcementQueryService;

        public AnnouncementController(ICommandService commandService,IContextService contextService,
            IAnnouncementQueryService announcementQueryService):base(commandService,contextService)
        {
            _announcementQueryService = announcementQueryService;
        }

        #region 公告列表

        /// <summary>
        /// 产品列表页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse List(ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var announcements = _announcementQueryService.ListPage();
            var total = announcements.Count();

            announcements = announcements.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new ListResponse
            {
                Total = total,
                Announcements = announcements.Select(x=>new Announcement {
                    Id=x.Id,
                    Title=x.Title,
                    Body=x.Body,
                    CreatedOn=x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }

        /// <summary>
        /// 获取最新公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse LatestAnnouncement()
        {
            var announcement = _announcementQueryService.ListPage().OrderByDescending(x=>x.CreatedOn).Take(1).SingleOrDefault();
            if(announcement==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有公告" };
            }
            return new LatestAnnouncementResponse
            {
                Announcement = new Announcement
                {
                    Id = announcement.Id,
                    Title = announcement.Title,
                    Body = announcement.Body,
                    CreatedOn=announcement.CreatedOn.GetTimeSpan()
                }
            };
        }

        #endregion
        
    }
}