using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request.Goodses;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Goodses;
using Shop.Commands.Goodses;
using Shop.Commands.Goodses.Specifications;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        public BaseApiResponse SearchGoodses(SearchGoodsesRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodses =_goodsQueryService.Goodses().Where(x=>x.IsPublished && x.Status==GoodsStatus.Verifyed && x.Name.Contains(request.Name));
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
        public BaseApiResponse CategoryGoods(CategoryGoodsRequest request)
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
        public BaseApiResponse ListPage(ListPageRequest request)
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
            if (request.IsPublished != -1)
            {
                goodses = goodses.Where(x => x.IsPublished == Convert.ToBoolean(request.IsPublished));
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
        public async Task<BaseApiResponse> UpdateGoods(UpdateGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goods = _goodsQueryService.GetGoodsAlias(request.Id);
            if(goods==null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到该商品" };
            }
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "审核商品", request.Id, goods.Name);

            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public BaseApiResponse GoodsSpecifications(GetParamsRequest request)
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
        public async Task<BaseApiResponse> Publish(PublishRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goods = _goodsQueryService.GetGoodsAlias(request.Id);
            if(goods==null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到商品" };
            }
            var command = new PublishGoodsCommand
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
            RecordOperat(currentAdmin.AdminId.ToGuid(), "上架商品", request.Id, goods.Name);
            return new BaseApiResponse();
        }
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> UnPublish(PublishRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goods = _goodsQueryService.GetGoodsAlias(request.Id);
            if (goods == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到商品" };
            }
            var command = new UnpublishGoodsCommand
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
            RecordOperat(currentAdmin.AdminId.ToGuid(), "下架商品", request.Id, goods.Name);
            return new BaseApiResponse();
        }
        #endregion


        
    }
}