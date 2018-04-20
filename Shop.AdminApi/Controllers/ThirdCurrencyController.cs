using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.ThirdCurrencys;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.ThirdCurrencys;
using Shop.Commands.ThirdCurrencys;
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
        public async Task<BaseApiResponse> Add(AddThirdCurrencyRequest request)
        {
            request.CheckNotNull(nameof(request));

            var newthirdcurrencyid = GuidUtil.NewSequentialId();
            var command = new CreateThirdCurrencyCommand(
                newthirdcurrencyid,
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加外币",newthirdcurrencyid, request.Name);
            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditThirdCurrencyRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑外币", request.Id, request.Name);
            return new BaseApiResponse();
        }

        /// <summary>
        /// 调整最大可导入量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> AdjustMaxImportAmount(AdjustMaxImportAmountRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "调整外币最大导入量", request.Id, "{0},{1}{2}".FormatWith(thirdCurrency.Name,request.Direction.ToDescription(),request.Amount));
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest request)
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除外币", request.Id, thirdCurrency.Name);

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