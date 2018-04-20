using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Announcements;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Announcements;
using Shop.Commands.Announcements;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        public async Task<BaseApiResponse> Add(AddAnnouncementRequest request)
        {
            request.CheckNotNull(nameof(request));

            var newannouncementid = GuidUtil.NewSequentialId();
            var command = new CreateAnnouncementCommand(
                newannouncementid,
                request.Title,
                request.Body
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "发布公告", newannouncementid, request.Title);

            return new BaseApiResponse();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditAnnouncementRequest request)
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

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑公告", request.Id, request.Title);

            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除公告", request.Id, announcement.Title);

            return new BaseApiResponse();
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse ListPage(ListPageRequest request)
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