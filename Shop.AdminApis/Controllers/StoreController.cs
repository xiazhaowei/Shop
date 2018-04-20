using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request.Store;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Store;
using Shop.Commands.Stores;
using Shop.Common.Enums;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class StoreController:BaseApiController
    {

        private IStoreQueryService _storeQueryService;//Q端
        private IGoodsQueryService _goodsQueryService;
        private IStoreOrderQueryService _storeOrderQueryService;
        
        public StoreController(ICommandService commandService, IContextService contextService,
            IStoreQueryService storeQueryService, 
            IStoreOrderQueryService storeOrderQueryService,
            IGoodsQueryService goodsQueryService) : base(commandService,contextService)
        {
            _storeQueryService = storeQueryService;
            _storeOrderQueryService = storeOrderQueryService;
            _goodsQueryService = goodsQueryService;
        }
        

        #region 总后台管理
        /// <summary>
        /// 店铺目前销售额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("TotalTodaySale")]
        public BaseApiResponse TotalTodaySale()
        {
            var todaySale = _storeQueryService.TodaySale();
            return new TotalTodaySaleResponse
            {
                TotalTodaySale = todaySale
            };
        }
        
        /// <summary>
        /// 店铺列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public ListPageResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var stores = _storeQueryService.StoreList();
            var total = stores.Count();
            //筛选
            if (request.Type != StoreType.All)
            {
                stores = stores.Where(x => x.Type == request.Type);
            }
            if (request.Status != StoreStatus.All)
            {
                stores = stores.Where(x => x.Status == request.Status);
            }
            if (!request.Name.IsNullOrEmpty())
            {
                stores = stores.Where(x => x.Name.Contains(request.Name));
            }
            if (!request.Region.IsNullOrEmpty())
            {
                stores = stores.Where(x => x.Region.Contains(request.Region));
            }
            total = stores.Count();

            //分页
            stores = stores.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            
            return new ListPageResponse
            {
                Total = total,
                Stores = stores.Select(x => new Store
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Mobile=x.Mobile,
                    Name = x.Name,
                    Region = x.Region,
                    Address = x.Address,
                    TodayOrder=x.TodayOrder,
                    OnSaleGoodsCount=x.OnSaleGoodsCount,
                    TodaySale=x.TodaySale,
                    Description=x.Description,
                    TotalOrder=x.TotalOrder,
                    TotalSale=x.TotalSale,

                    SubjectName=x.SubjectName,
                    SubjectNumber=x.SubjectNumber,
                    SubjectPic=x.SubjectPic,
                    Type=x.Type.ToString(),
                    Status = x.Status.ToString()
                }).ToList()
            };
        }

        /// <summary>
        /// 修改店铺信息 后台修改 可以修改店铺类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]AdminEditRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateStoreCommand(
                request.Name,
                request.Description,
                request.Address,
                request.Type)
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
        /// 编辑商家状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("EditStatus")]
        public async Task<BaseApiResponse> EditStatus([FromBody]EditStatusRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateStoreStautsCommand(
                request.Status)
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
        #endregion

        

    }
}