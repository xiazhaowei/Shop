using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.GoodsBlocks;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.GoodsBlocks;
using Shop.Api.Models.Response.Goodses;
using Shop.Commands.GoodsBlocks;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class GoodsBlockController : BaseApiController
    {
        private IGoodsBlockQueryService _goodsBlockQueryService;//Q 端
        private IGoodsBlockWarpQueryService _goodsBlockWarpQueryService;

        public GoodsBlockController(ICommandService commandService,IContextService contextService,
            IGoodsBlockQueryService goodsBlockQueryService,
            IGoodsBlockWarpQueryService goodsBlockWarpQueryService) : base(commandService,contextService)
        {
            _goodsBlockQueryService = goodsBlockQueryService;
            _goodsBlockWarpQueryService = goodsBlockWarpQueryService;
        }
        

        #region 后台管理
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
            var goodsBlocks = _goodsBlockQueryService.All();
            var total = goodsBlocks.Count();
            try
            {
                goodsBlocks = goodsBlocks.OrderByDescending(x => x.Sort).Skip(pageSize * (request.Page - 1)).Take(pageSize);
                return new ListResponse
                {
                    Total = total,
                    GoodsBlocks = goodsBlocks.Select(x => new GoodsBlock
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Thumb=x.Thumb,
                        Banner=x.Banner,
                        Layout = x.Layout.ToString(),
                        IsShow = x.IsShow,
                        Goodses=x.Goodses.Select(z=>new Goods {
                            Id=z.Id,
                            Name=z.Name,
                            Price=z.Price,
                            OriginalPrice=z.OriginalPrice,
                            Benevolence=z.Benevolence,
                            Pics=z.Pics.IsNullOrEmpty() ? null : z.Pics.Split("|",true),
                            CreatedOn=z.CreatedOn,
                            Rate=z.Rate,
                            SellOut=z.SellOut
                        }).ToList(),
                        Sort = x.Sort
                    }).ToList()
                };
            }
            catch(Exception e)
            {
                return new BaseApiResponse { Code = 400, Message = e.Source +e.StackTrace+e.Message};
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GoodsBlocks")]
        public BaseApiResponse GoodsBlocks()
        {
            var goodsBlocks = _goodsBlockQueryService.All().Where(x=>x.IsShow);
            var total = goodsBlocks.Count();
            try
            {
                return new ListResponse
                {
                    Total = total,
                    GoodsBlocks = goodsBlocks.Select(x => new GoodsBlock
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Thumb = x.Thumb,
                        Banner = x.Banner,
                        Layout = x.Layout.ToString(),
                        IsShow = x.IsShow,
                        Goodses = x.Goodses.Select(z => new Goods
                        {
                            Id = z.Id,
                            Name = z.Name,
                            Price = z.Price,
                            OriginalPrice = z.OriginalPrice,
                            Benevolence = z.Benevolence,
                            Pics = z.Pics.IsNullOrEmpty() ? null : z.Pics.Split("|", true),
                            CreatedOn = z.CreatedOn,
                            Rate = z.Rate,
                            SellOut = z.SellOut
                        }).ToList(),
                        Sort = x.Sort
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                return new BaseApiResponse { Code = 400, Message = e.Source + e.StackTrace + e.Message };
            }
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddGoodsBlockRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateGoodsBlockCommand(
                GuidUtil.NewSequentialId(),
                request.Name,
                request.Thumb,
                request.Banner,
                request.Layout,
                request.Goodses,
                request.IsShow,
                request.Sort
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
        public async Task<BaseApiResponse> Edit([FromBody]EditGoodsBlockRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var goodsBlock = _goodsBlockQueryService.Find(request.Id);
            if (goodsBlock == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该项目" };
            }

            var command = new UpdateGoodsBlockCommand(
                request.Name,
                request.Thumb,
                request.Banner,
                request.Layout,
                request.Goodses,
                request.IsShow,
                request.Sort)
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
            var goodsBlock = _goodsBlockQueryService.Find(request.Id);
            if (goodsBlock == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该项目" };
            }
            //删除
            var command = new DeleteGoodsBlockCommand
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
