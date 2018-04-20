using ENode.Commanding;
using Shop.Api.Models.Request;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.GoodsBlocks;
using Shop.Api.Models.Response.Goodses;
using Shop.Api.Services;
using Shop.Common.Enums;
using Shop.QueryServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
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

        /// <summary>
        /// 主页block
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse HomeGoodsBlockWarps()
        {
            var goodsBlockWarps = _goodsBlockWarpQueryService.AllAlis().Where(x=>x.IsShow).OrderByDescending(x=>x.Sort);

            return new HomeGoodsBlockWarpsResponse
            {
                GoodsBlockWarps = goodsBlockWarps.Select(x => new GoodsBlockWarp
                {
                    Id = x.Id,
                    Name=x.Name,
                    Style = x.Style.ToString(),
                    Sort = x.Sort,
                    IsShow = x.IsShow,
                    GoodsBlocks = getGoodsBlocks(x)
                }).ToList()
            };
        }

        /// <summary>
        /// 产品簇详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse Info(InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodsBlock = _goodsBlockQueryService.Find(request.Id);
            if (goodsBlock == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到该产品板块" };
            }
            return new GoodsBlockInfoResponse
            {
                GoodsBlock = new GoodsBlock
                {
                    Id = goodsBlock.Id,
                    Name = goodsBlock.Name,
                    Thumb = goodsBlock.Thumb,
                    Banner = goodsBlock.Banner,
                    Layout=goodsBlock.Layout.ToString(),
                    IsShow = goodsBlock.IsShow,
                    Sort = goodsBlock.Sort,
                    Goodses = goodsBlock.Goodses.Select(x => new Goods
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Pics = x.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                        Price = x.Price,
                        OriginalPrice = x.OriginalPrice,
                        Benevolence = x.Benevolence,
                        SellOut = x.SellOut,
                        Rate=x.Rate,
                        CreatedOn=x.CreatedOn
                    }).ToList()
                }
            };
        }
        

        #region 私有方法
        private IList<GoodsBlock> getGoodsBlocks(QueryServices.Dtos.GoodsBlockWarpAlis goodsBlockWarpAlis)
        {
            var goodsBlocks = _goodsBlockQueryService.GetList(goodsBlockWarpAlis.Id);
            return goodsBlocks.Where(x => x.IsShow).OrderByDescending(x => x.Sort).Select(x => new GoodsBlock
            {
                Id = x.Id,
                Name = x.Name,
                Thumb = x.Thumb,
                Banner = x.Banner,
                Layout = x.Layout.ToString(),
                IsShow = x.IsShow,
                Goodses = ((goodsBlockWarpAlis.Style == GoodsBlockWarpStyle.SingleColThumWithGoods
                || goodsBlockWarpAlis.Style == GoodsBlockWarpStyle.SingleLineGoods 
                || goodsBlockWarpAlis.Style==GoodsBlockWarpStyle.ColGoods) ? x.Goodses.Select(z => new Goods
                {
                    Id = z.Id,
                    Name = z.Name,
                    Pics = z.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Price = z.Price,
                    OriginalPrice = z.OriginalPrice,
                    SellOut = z.SellOut,
                    Rate = z.Rate,
                    CreatedOn = z.CreatedOn,
                    Benevolence = z.Benevolence
                }).ToList() : null),
                Sort = x.Sort
            }).ToList();
        }


        
        #endregion
    }
}
