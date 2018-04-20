using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Partners;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Partners;
using Shop.Commands.Partners;
using Shop.Common.Enums;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
    public class PartnerController:BaseApiController
    {
        private IPartnerQueryService _partnerQueryService;
        private IUserQueryService _userQueryService;

        public PartnerController(ICommandService commandService, IContextService contextService,
            IPartnerQueryService partnerQueryService,
            IUserQueryService userQueryService):base(commandService,contextService)
        {
            _partnerQueryService = partnerQueryService;
            _userQueryService = userQueryService;
        }
        
        #region 后台管理


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Add(AddPartnerRequest request)
        {
            request.CheckNotNull(nameof(request));

            var userInfo = _userQueryService.FindUser(request.Mobile);
            if(userInfo==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }
            var newpartnerid = GuidUtil.NewSequentialId();
            var command = new CreatePartnerCommand(
                newpartnerid,
                userInfo.Id,
                userInfo.WalletId,
                request.Mobile,
                request.Region,
                request.Level,
                request.Persent,
                request.CashPersent,
                request.BalanceInterval,
                request.Remark,
                request.IsLocked
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加代理", newpartnerid, "{0}代理地区{1}".FormatWith(userInfo.Mobile,request.Region));

            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> AcceptNewBalance(AcceoptNewBalanceRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (request.Amount <= 0)
            {
                return new BaseApiResponse { Code = 400, Message = "地区销售额错误" };
            }
            var partner = _partnerQueryService.Find(request.Id);
            if(partner==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到代理" };
            }
            var command = new AcceptNewBalanceCommand(request.Amount)
            {
                AggregateRootId = request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "给代理分红", request.Id, "分红地区：{0}，销售额：{1}".FormatWith(partner.Region, request.Amount));

            return new BaseApiResponse();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditPartnerRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var partner = _partnerQueryService.Find(request.Id);
            if (partner == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该代理" };
            }

            var command = new UpdatePartnerCommand(
                request.Mobile,
                request.Region,
                request.Level,
                request.Persent,
                request.CashPersent,
                request.BalanceInterval,
                request.Remark,
                request.IsLocked
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
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑代理", request.Id,request.Region);

            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest  request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var partner = _partnerQueryService.Find(request.Id);
            if (partner == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }
            //删除
            var command = new DeletePartnerCommand {
                AggregateRootId= request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除代理", request.Id, partner.Region);

            return new BaseApiResponse();
        }

        

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse ListPage(ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var partners = _partnerQueryService.Partners();
            var total = partners.Count();
            //筛选
            if(request.Level!=PartnerLevel.All)
            {
                partners = partners.Where(x => x.Level==request.Level);
            }
            if(!request.Mobile.IsNullOrEmpty())
            {
                partners = partners.Where(x => x.Mobile.Contains(request.Mobile));
            }
            if (!request.Region.IsNullOrEmpty())
            {
                partners = partners.Where(x=>x.Region.Contains(request.Region));
            }
            //分页
            partners = partners.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new ListResponse
            {
                Total = total,
                Partners = partners.Select(x => new Partner
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    Mobile=x.Mobile,
                    Region=x.Region,
                    Level=x.Level.ToString(),
                    Persent=x.Persent,
                    CashPersent = x.CashPersent,
                    BalanceInterval=x.BalanceInterval,
                    LastBalancedAmount=x.LastBalancedAmount,
                    TotalBalancedAmount=x.TotalBalancedAmount,
                    BalancedDate =x.BalancedDate.ToShortDateString(),
                    NextBalancedDate=x.BalancedDate.AddDays(x.BalanceInterval).ToShortDateString(),
                    CreatedOn=x.CreatedOn.GetTimeSpan(),
                    Remark=x.Remark,
                    IsLocked=x.IsLocked
                }).ToList()
            };
        }

        #endregion
    }
}