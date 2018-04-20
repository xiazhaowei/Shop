using ENode.Commanding;
using Shop.Api.Extensions;
using Shop.Api.Models.Request.ThirdCurrencys;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.ThirdCurrencys;
using Shop.Api.Services;
using Shop.Commands.ThirdCurrencys;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class ThirdCurrencyController:BaseApiController
    {
        private IThirdCurrencyQueryService _thirdCurrencyQueryService;
        private IUserQueryService _userQueryService;

        public ThirdCurrencyController(ICommandService commandService,IContextService contextService,
            IThirdCurrencyQueryService thirdCurrencyQueryService,
            IUserQueryService userQueryService
            ):base(commandService,contextService)
        {
            _thirdCurrencyQueryService = thirdCurrencyQueryService;
            _userQueryService = userQueryService;
        }

        /// <summary>
        /// 可转入的外币列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ThirdCurrencysResponse ThirdCurrencys()
        {
            var thirdCurrencys = _thirdCurrencyQueryService.ThirdCurrencys().Where(x=>!x.IsLocked);
            return new ThirdCurrencysResponse
            {
                ThirdCurrencys = thirdCurrencys.Select(x => new ThirdCurrency
                {
                    Id = x.Id,
                    Name = x.Name,
                    CompanyName = x.CompanyName,
                    Icon = x.Icon,
                    Conversion = x.Conversion,
                    ImportedAmount = x.ImportedAmount,
                    MaxImportAmount=x.MaxImportAmount,
                    Remark=x.Remark,
                    IsLocked = x.IsLocked,
                    CreatedOn = x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }

        /// <summary>
        /// 外币导入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ImportCurrency(ImportCurrencyRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());

            var thirdCurrency = _thirdCurrencyQueryService.Find(request.Id);
            if(thirdCurrency==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到该外币" };
            }
            if(thirdCurrency.IsLocked)
            {
                return new BaseApiResponse { Code = 400, Message = "外币锁定，无法导入" };
            }
            //这里要访问第三方系统账号
            return new BaseApiResponse { Code = 400, Message = "接口未开放，暂时无法导入" };
            //外币导入
            var command = new AcceptNewImportCommand(
                userInfo.WalletId,
                userInfo.Id,
                userInfo.Mobile,
                request.Account,
                request.Amount
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


        [HttpPost]
        [Authorize]
        public BaseApiResponse ImportLogs(ImportLogsRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            //获取数据
            int pageSize = 10;

            var importLogs = _thirdCurrencyQueryService.ThirdCurrencyImportLogs(request.Id, userInfo.WalletId);
            var total = importLogs.Count();
            //分页
            importLogs = importLogs.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new ImportLogsResponse
            {
                Total = total,
                ImportLogs = importLogs.Select(x => new ThirdCurrencyImportLog
                {
                    Id = x.Id,
                    ThirdCurrencyId = x.ThirdCurrencyId,
                    WalletId = x.WalletId,
                    Mobile = x.Mobile,
                    Account = x.Account,
                    Amount = x.Amount,
                    ShopCashAmount = x.ShopCashAmount,
                    Conversion = x.Conversion,
                    CreatedOn = x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }
        
    }
}