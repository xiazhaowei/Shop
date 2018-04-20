using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request.Goodses;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Goodses;
using Shop.Commands.Goodses;
using Shop.Commands.Goodses.Specifications;
using Shop.Common.Enums;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class GoodsController:BaseApiController
    {
        private IStoreQueryService _storeQueryService;//Q端
        private IGoodsQueryService _goodsQueryService;

        public GoodsController(ICommandService commandService,IContextService contextService,
            IStoreQueryService storeQueryService, 
            IGoodsQueryService goodsQueryService) : base(commandService,contextService)
        {
            _storeQueryService = storeQueryService;
            _goodsQueryService = goodsQueryService;
        }

        

        #region 总后台管理

        [HttpPost]
        [Authorize]
        [Route("SearchGoodses")]
        public BaseApiResponse SearchGoodses([FromBody]SearchGoodsesRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodses =_goodsQueryService.Goodses().Where(x=>x.Name.Contains(request.Name));
            return new GoodsAlisesResponse
            {
                Goodses = goodses.Select(x => new GoodsAlis
                {
                    Id = x.Id,
                    Name = x.Name,
                    Pic = x.Pics.Split("|", true).Length > 0 ? x.Pics.Split("|", true)[0] : "",
                    Price = x.Price
                }).ToList()
            };
        }


        [HttpPost]
        [Authorize]
        [Route("CategoryGoods")]
        public BaseApiResponse CategoryGoods([FromBody]CategoryGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var goodses = _goodsQueryService.CategoryGoodses(request.Id);
            return new GoodsAlisesResponse
            {
                Goodses = goodses.Select(x => new GoodsAlis
                {
                    Id = x.Id,
                    Name = x.Name,
                    Pic = x.Pics.Split("|", true).Length>0? x.Pics.Split("|", true)[0] : "",
                    Price = x.Price
                }).ToList()
            };
        }
        /// <summary>
        /// 所有商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public BaseApiResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var goodses = _goodsQueryService.Goodses();
            var pageSize = 20;
            var total = goodses.Count();
            //筛选
            if (request.Status != GoodsStatus.All)
            {
                goodses = goodses.Where(x => x.Status == request.Status);
            }
            if (!request.Name.IsNullOrEmpty())
            {
                goodses = goodses.Where(x => x.Name.Contains(request.Name));
            }
            if (!request.StoreName.IsNullOrEmpty())
            {
                goodses = goodses.Where(x => x.StoreName.Contains(request.StoreName));
            }
            total = goodses.Count();
            //分页
            goodses = goodses.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new GoodsesResponse
            {
                Total = total,
                Goodses = goodses.Select(x => new GoodsDetails
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    Pics = x.Pics.Split("|", true).ToList(),
                    StoreName=x.StoreName,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Stock = x.Stock,
                    OriginalPrice = x.OriginalPrice,
                    Benevolence = x.Benevolence,
                    Is7SalesReturn = x.Is7SalesReturn,
                    IsInvoice = x.IsInvoice,
                    IsPayOnDelivery = x.IsPayOnDelivery,
                    CreatedOn = x.CreatedOn,
                    SellOut=x.SellOut,
                    Sort = x.Sort,
                    IsPublished =x.IsPublished,
                    Status=x.Status.ToString(),
                    RefusedReason=x.RefusedReason
                }).ToList()
            };
        }

        /// <summary>
        /// 后台更新商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UpdateGoods")]
        public async Task<BaseApiResponse> UpdateGoods([FromBody]UpdateGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateGoodsCommand(
                request.Name,
                request.Description,
                request.Pics,
                request.Price,
                request.Benevolence,
                request.SellOut,
                request.Status,
                request.RefusedReason)
            {
                AggregateRootId = request.Id
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "更新商品失败：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //更新规格
            var command2 = new UpdateSpecificationsCommand(
                request.Id,
                request.Specifications.Select(x => new Commands.Goodses.Specifications.SpecificationInfo(
                    x.Id,
                    x.Name,
                    x.Value,
                    x.Thumb,
                    x.Price,
                    x.OriginalPrice,
                    x.Benevolence,
                    x.Number,
                    x.BarCode,
                    x.Stock
                    )).ToList(),
                true);
            var result2 = await ExecuteCommandAsync(command2);
            if (!result2.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result2.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        [Route("GetSpecifications")]
        public BaseApiResponse GetSpecifications([FromBody]GetParamsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var specifications = _goodsQueryService.GetPublishedSpecifications(request.Id);
            if (!specifications.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "没有参数" };
            }
            return new GetSpecificationsResponse
            {
                Specifications = specifications.Select(x => new Specification
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                    Benevolence=x.Benevolence,
                    Number = x.Number,
                    BarCode = x.BarCode,
                    Stock = x.Stock,
                    Thumb = x.Thumb,
                    AvailableQuantity=x.AvailableQuantity
                }).ToList()
            };
        }
       

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Publish")]
        public async Task<BaseApiResponse> Publish([FromBody]PublishRequest request)
        {
            request.CheckNotNull(nameof(request));
            var command = new PublishGoodsCommand
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
        /// 下架商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UnPublish")]
        public async Task<BaseApiResponse> UnPublish([FromBody]PublishRequest request)
        {
            request.CheckNotNull(nameof(request));
            var command = new UnpublishGoodsCommand
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