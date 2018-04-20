using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
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
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        public BaseApiResponse ListPage(ListPageRequest request)
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
        public async Task<BaseApiResponse> Add(AddGoodsBlockRequest request)
        {
            request.CheckNotNull(nameof(request));

            var newgoodsblockid = GuidUtil.NewSequentialId();
            var command = new CreateGoodsBlockCommand(
                newgoodsblockid,
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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加产品簇", newgoodsblockid, request.Name);

            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditGoodsBlockRequest request)
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

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑产品簇", request.Id, request.Name);

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
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除产品簇", request.Id, goodsBlock.Name);
            return new BaseApiResponse();
        }
        #endregion
        
    }
}
