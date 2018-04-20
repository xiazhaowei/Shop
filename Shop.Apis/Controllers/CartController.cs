using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request.Carts;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Carts;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Commands.Carts;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class CartController:BaseApiController
    {
        private ICartQueryService _cartQueryService;//Q 端
        private IUserQueryService _userQueryService;

        public CartController(ICommandService commandService, IContextService contextService,
            IUserQueryService userQueryService,
            ICartQueryService cartQueryService) : base(commandService,contextService)
        {
            _userQueryService = userQueryService;
            _cartQueryService = cartQueryService;
        }

        /// <summary>
        /// 我的购物车信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Info")]
        public BaseApiResponse Info()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            if(userInfo==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到用户" };
            }
            var cartGoodses = _cartQueryService.CartGoodses(userInfo.CartId);
            var storeCartGoodses = cartGoodses.GroupBy(x => x.StoreId).Select(g => new
            {
                StoreId = g.Key,
                Goodses = g.Select(gs => new
                {
                    Id = gs.Id,
                    GoodsId = gs.GoodsId,
                    StoreId = gs.StoreId,
                    SpecificationId = gs.SpecificationId,
                    StoreName=gs.StoreName,
                    SpecificationName = gs.SpecificationName,
                    GoodsName = gs.GoodsName,
                    GoodsPic=gs.GoodsPic,
                    Stock = gs.Stock,
                    Price = gs.Price,
                    OriginalPrice=gs.OriginalPrice,
                    Quantity = gs.Quantity,
                    Benevolence = gs.Benevolence,
                    IsGoodsPublished=gs.IsGoodsPublished,
                    GoodsStatus=gs.GoodsStatus
                })
            });

            return new CartInfoResponse
            {
                StoreCartGoods = storeCartGoodses.Select(x => new StoreCartGoods
                {
                    StoreId = x.StoreId,
                    StoreName = x.Goodses.First().StoreName,
                    CartGoodses = x.Goodses.Select(cg => new CartGoods
                    {
                        Id = cg.Id,
                        StoreId = cg.StoreId,
                        GoodsId = cg.GoodsId,
                        SpecificationId = cg.SpecificationId,
                        GoodsName = cg.GoodsName,
                        GoodsPic=cg.GoodsPic,
                        SpecificationName = cg.SpecificationName,
                        Price = cg.Price,
                        OriginalPrice=cg.OriginalPrice,
                        Quantity = cg.Quantity,
                        Stock = cg.Stock,
                        Benevolence = cg.Benevolence,
                        IsGoodsPublished=cg.IsGoodsPublished,
                        GoodsStatus=cg.GoodsStatus.ToString(),
                        Checked=false
                    }).ToList()
                }).ToList()
            };
        }

        /// <summary>
        /// 添加购物车商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddCartGoods")]
        public async Task<BaseApiResponse> AddCartGoods([FromBody]AddCartGoodsRequest request)
        {
            //获取用户的购物车后台获取
            request.CheckNotNull(nameof(request));
            
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());

            var command = new AddCartGoodsCommand(
                request.StoreId,
                request.GoodsId,
                request.SpecificationId,
                request.GoodsName,
                request.GoodsPic,
                request.SpecificationName,
                request.Price,
                request.OriginalPrice,
                request.Quantity,
                request.Stock,
                request.Benevolence)
            {
                AggregateRootId = userInfo.CartId
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("RemoveCartGoods")]
        public async Task<BaseApiResponse> RemoveCartGoods([FromBody]RemoveCartGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());

            var command = new RemoveCartGoodsCommand(request.Id)
            {
                AggregateRootId = userInfo.CartId
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        
    }
}