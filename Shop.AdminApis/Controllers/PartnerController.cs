using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Partners;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Partners;
using Shop.Commands.Partners;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
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
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddPartnerRequest request)
        {
            request.CheckNotNull(nameof(request));

            var userInfo = _userQueryService.FindUser(request.Mobile);
            if(userInfo==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }

            var command = new CreatePartnerCommand(
                GuidUtil.NewSequentialId(),
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
            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]EditPartnerRequest request)
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
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest  request)
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
            return new BaseApiResponse();
        }

        

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public BaseApiResponse ListPage([FromBody]ListPageRequest request)
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
                    CreatedOn=x.CreatedOn.GetTimeSpan(),
                    Remark=x.Remark,
                    IsLocked=x.IsLocked
                }).ToList()
            };
        }

        #endregion
    }
}