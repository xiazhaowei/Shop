using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.ThirdCurrencys;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.ThirdCurrencys;
using Shop.Commands.ThirdCurrencys;
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
    public class ThirdCurrencyController:BaseApiController
    {
        private IThirdCurrencyQueryService _thirdCurrencyQueryService;

        public ThirdCurrencyController(ICommandService commandService,IContextService contextService,
            IThirdCurrencyQueryService thirdCurrencyQueryService
            ):base(commandService,contextService)
        {
            _thirdCurrencyQueryService = thirdCurrencyQueryService;
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
        public async Task<BaseApiResponse> Add([FromBody]AddThirdCurrencyRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateThirdCurrencyCommand(
                GuidUtil.NewSequentialId(),
                request.Name,
                request.Icon,
                request.CompanyName,
                request.Conversion,
                request.IsLocked,
                request.Remark
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
        public async Task<BaseApiResponse> Edit([FromBody]EditThirdCurrencyRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var thirdCurrency = _thirdCurrencyQueryService.Find(request.Id);
            if (thirdCurrency == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该外币" };
            }

            var command = new UpdateThirdCurrencyCommand(
                request.Name,
                request.Icon,
                request.CompanyName,
                request.Conversion,
                request.IsLocked,
                request.Remark
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
        /// 调整最大可导入量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AdjustMaxImportAmount")]
        public async Task<BaseApiResponse> AdjustMaxImportAmount([FromBody]AdjustMaxImportAmountRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var thirdCurrency = _thirdCurrencyQueryService.Find(request.Id);
            if (thirdCurrency == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该外币" };
            }
            var amount = request.Amount;
            if (request.Direction == WalletDirection.Out)
            {
                amount = -amount;
            }
            var command = new ChargeThirdCurrencyCommand(
                amount
                )
            {
                AggregateRootId = request.Id
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
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var thirdCurrency = _thirdCurrencyQueryService.Find(request.Id);
            if (thirdCurrency == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该外币" };
            }
            //删除
            var command = new DeleteThirdCurrencyCommand {
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
            var thirdCurrencys = _thirdCurrencyQueryService.ThirdCurrencys();
            var total = thirdCurrencys.Count();
            //筛选
            
            if(!request.Name.IsNullOrEmpty())
            {
                thirdCurrencys = thirdCurrencys.Where(x => x.Name.Contains(request.Name));
            }

            //分页
            thirdCurrencys = thirdCurrencys.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new ListResponse
            {
                Total = total,
                ThirdCurrencys = thirdCurrencys.Select(x => new ThirdCurrency
                {
                    Id = x.Id,
                    Name=x.Name,
                    Icon=x.Icon,
                    CompanyName=x.CompanyName,
                    Conversion = x.Conversion,
                    ImportedAmount=x.ImportedAmount,
                    MaxImportAmount=x.MaxImportAmount,
                    CreatedOn=x.CreatedOn.GetTimeSpan(),
                    Remark=x.Remark,
                    IsLocked=x.IsLocked
                }).ToList()
            };
        }

        #endregion
    }
}