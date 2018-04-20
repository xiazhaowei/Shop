using ENode.Commanding;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request.Announcements;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Announcements;
using Shop.Apis.Services;
using Shop.QueryServices;
using System.Linq;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    /// <summary>
    /// 公告
    /// </summary>
    [Route("[controller]")]
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
        [Route("List")]
        public BaseApiResponse List([FromBody]ListPageRequest request)
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
                    Body=x.Body
                }).ToList()
            };
        }

        /// <summary>
        /// 获取最新公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("LatestAnnouncement")]
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
                    Body = announcement.Body
                }
            };
        }

        #endregion
        
    }
}