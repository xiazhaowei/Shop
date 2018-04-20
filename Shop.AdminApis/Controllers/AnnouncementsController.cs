using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Announcements;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Announcements;
using Shop.Commands.Announcements;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class AnnouncementController : BaseApiController
    {
        private IAnnouncementQueryService _announcementQueryService;

        public AnnouncementController(ICommandService commandService,IContextService contextService,
            IAnnouncementQueryService announcementQueryService):base(commandService,contextService)
        {
            _announcementQueryService = announcementQueryService;
        }
        
        #region 后台管理

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddAnnouncementRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateAnnouncementCommand(
                GuidUtil.NewSequentialId(),
                request.Title,
                request.Body
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]EditAnnouncementRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateAnnouncementCommand(
                request.Title,
                request.Body
                )
            {
                AggregateRootId=request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var announcement = _announcementQueryService.Find(request.Id);
            if (announcement == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该公告" };
            }
            //删除
            var command = new DeleteAnnouncementCommand(request.Id);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public BaseApiResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var announcements = _announcementQueryService.ListPage();
            var total = announcements.Count();

            announcements = announcements.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new ListResponse
            {
                Total = total,
                Announcements = announcements.Select(x => new Announcement
                {
                    Id = x.Id,
                    Title = x.Title,
                    Body = x.Body
                }).ToList()
            };
        }
        #endregion

        
    }
}