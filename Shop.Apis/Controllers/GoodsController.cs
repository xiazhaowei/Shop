using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Goodses;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Goodses;
using Shop.Api.Models.Response.Store;
using Shop.Apis.Extensions;
using Shop.Apis.Helpers;
using Shop.Apis.Services;
using Shop.Commands.Comments;
using Shop.Commands.Goodses;
using Shop.Commands.Goodses.Specifications;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class GoodsController:BaseApiController
    {
        private IStoreQueryService _storeQueryService;//Q端
        private IGoodsQueryService _goodsQueryService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoodsController(IHostingEnvironment hostingEnvironment,ICommandService commandService,IContextService contextService,
            IStoreQueryService storeQueryService, 
            IGoodsQueryService goodsQueryService) : base(commandService,contextService)
        {
            _storeQueryService = storeQueryService;
            _goodsQueryService = goodsQueryService;
            _hostingEnvironment = hostingEnvironment;
        }


        /// <summary>
        /// 产品列表页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GoodsList")]
        public BaseApiResponse GoodsList([FromBody]GoodsListRequest request)
        {
            request.CheckNotNull(nameof(request));
            int pageSize = 10;
            IEnumerable<QueryServices.Dtos.GoodsAlias> goodses = null;
            if (request.Type.Equals("Search"))
            {
                goodses = new SearchEngine(_hostingEnvironment).SearchGoods(request.Search, "", null, null);
            }
            if (request.Type.Equals("Category"))
            {
                goodses = _goodsQueryService.CategoryGoodses(request.CategoryId);
            }
            //排序
            if (request.Sort.Equals("销量"))
            {
                goodses = goodses.OrderByDescending(x => x.SellOut);//根据销量
            }
            else if (request.Sort.Equals("新品"))
            {
                goodses = goodses.OrderByDescending(x => x.CreatedOn);//根据发布时间
            }
            else
            {
                goodses = goodses.OrderByDescending(x => x.CreatedOn);
            }
            var total = goodses.Count();

            //分页
            var pageGoodses = goodses.Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new GoodsListResponse
            {
                Total = total,
                Goodses = pageGoodses.Select(x => new Goods
                {
                    Id = x.Id,
                    Pics = x.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Name = x.Name,
                    Price = x.Price,
                    OriginalPrice = x.OriginalPrice,
                    Benevolence = x.Benevolence,
                    SellOut = x.SellOut
                }).ToList()
            };
        }

        /// <summary>
        /// 首页产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("HomePageGoodses")]
        public BaseApiResponse HomePageGoodses()
        {
            //从缓存获取商品
            IEnumerable<QueryServices.Dtos.GoodsAlias> rateGoodses = _apiSession.GetHomeRateGoodses();
            if (rateGoodses == null)
            {
                rateGoodses = _goodsQueryService.GoodRateGoodses(12);
                _apiSession.SetHomeRateGoodses(rateGoodses);
            }

            IEnumerable<QueryServices.Dtos.GoodsAlias> newGoodses = _apiSession.GetHomeNewGoodses();
            if (newGoodses == null)
            {
                newGoodses = _goodsQueryService.NewGoodses(12);
                _apiSession.SetHomeNewGoodses(newGoodses);
            }

            IEnumerable<QueryServices.Dtos.GoodsAlias> selloutGoodses = _apiSession.GetHomeSelloutGoodses();
            if (selloutGoodses == null)
            {
                selloutGoodses = _goodsQueryService.GoodSellGoodses(12);
                _apiSession.SetHomeSelloutGoodses(selloutGoodses);
            }


            return new HomePageGoodsesResponse
            {
                RateGoodses = rateGoodses.Select(x => new Goods
                {
                    Id = x.Id,
                    Pics = x.Pics.Split("|", true).Select(img=>img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Name = x.Name,
                    Price = x.Price,
                    OriginalPrice=x.OriginalPrice,
                    Benevolence = x.Benevolence,
                    SellOut=x.SellOut
                }).ToList(),
                NewGoodses = newGoodses.Select(x => new Goods
                {
                    Id = x.Id,
                    Pics = x.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Name = x.Name,
                    Price = x.Price,
                    OriginalPrice=x.OriginalPrice,
                    Benevolence = x.Benevolence,
                    SellOut=x.SellOut
                }).ToList(),
                SellOutGoodses = selloutGoodses.Select(x => new Goods
                {
                    Id = x.Id,
                    Pics = x.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Name = x.Name,
                    Price = x.Price,
                    OriginalPrice=x.OriginalPrice,
                    Benevolence = x.Benevolence,
                    SellOut = x.SellOut
                }).ToList()
            };
        }
        
        /// <summary>
        /// 商品详情页面
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GoodsInfo")]
        public BaseApiResponse GoodsInfo([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));

            var goodsDetails = _goodsQueryService.GetGoodsDetails(request.Id);
            var storeInfo = _storeQueryService.Info(goodsDetails.StoreId);
            var specifications = _goodsQueryService.GetPublishedSpecifications(request.Id);
            var goodsParams = _goodsQueryService.GetGoodsParams(request.Id);
            var comments = _goodsQueryService.GetComments(request.Id, 5);
            if(goodsDetails==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有该产品" };
            }
            return new GoodsInfoResponse
            {
                GoodsDetails = new GoodsDetails
                {
                    Id = goodsDetails.Id,
                    StoreId = goodsDetails.StoreId,
                    Pics = goodsDetails.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                    Name = goodsDetails.Name,
                    Description = goodsDetails.Description,
                    Benevolence = goodsDetails.Benevolence,
                    Price = goodsDetails.Price,
                    OriginalPrice = goodsDetails.OriginalPrice,
                    Stock = goodsDetails.Stock,
                    Is7SalesReturn = goodsDetails.Is7SalesReturn,
                    IsInvoice = goodsDetails.IsInvoice,
                    IsPayOnDelivery = goodsDetails.IsPayOnDelivery,
                    Rate=goodsDetails.Rate,
                    QualityRate=goodsDetails.QualityRate,
                    ExpressRate=goodsDetails.ExpressRate,
                    DescribeRate=goodsDetails.DescribeRate,
                    PriceRate=goodsDetails.PriceRate,
                    RateCount=goodsDetails.RateCount,
                    Sort = goodsDetails.Sort,
                    IsPublished=goodsDetails.IsPublished,
                    Status=goodsDetails.Status.ToString()
                },
                StoreInfo=new StoreInfo
                {
                    Id=storeInfo.Id,
                    Name=storeInfo.Name,
                    Description=storeInfo.Description,
                    Type=storeInfo.Type.ToDescription(),
                    Address=storeInfo.Address
                },
                Specifications=specifications.Select(x=>new Specification {
                    Id=x.Id,
                    Name=x.Name,
                    Value=x.Value,
                    Price=x.Price,
                    OriginalPrice=x.OriginalPrice,
                    Benevolence=x.Benevolence,
                    Thumb=x.Thumb,
                    Stock=x.Stock,
                    BarCode=x.BarCode,
                    Number=x.Number,
                    AvailableQuantity=x.AvailableQuantity
                }).ToList(),
                GoodsParams=goodsParams.Select(x=>new GoodsParam {
                    Id=x.Id,
                    Name=x.Name,
                    Value=x.Value
                }).ToList(),
                Comments =comments.Select(x=>new Comment {
                    Id=x.Id,
                    Rate=x.Rate,
                    NickName=x.NickName,
                    CreatedOn=x.CreatedOn.GetTimeSpan(),
                    Thumbs=x.Thumbs.Split("|",true).ToList(),
                    Body=x.Body
                }).ToList()
            };
        }

        /// <summary>
        /// 评价商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Comment")]
        public async Task<BaseApiResponse> Comment([FromBody]CommentRequest request)
        {
            request.CheckNotNull(nameof(request));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var command = new CreateCommentCommand(
                GuidUtil.NewSequentialId(),
                request.GoodsId,
                currentAccount.UserId.ToGuid(),
                request.Body,
                request.Thumbs,
                request.PriceRate,
                request.DescribeRate,
                request.QualityRate,
                request.ExpressRate
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        #region 店铺商品管理
        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Publish")]
        public async Task<BaseApiResponse> GoodsPublish([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodsAlis = _goodsQueryService.GetGoodsAlias(request.Id);
            if(goodsAlis==null)
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
            return new BaseApiResponse();
        }
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnPublish")]
        public async Task<BaseApiResponse> GoodsUnPublish([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodsAlis = _goodsQueryService.GetGoodsAlias(request.Id);
            if (goodsAlis == null)
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
            return new BaseApiResponse();
        }
        /// <summary>
        /// 添加修改商品商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddGoods")]
        public async Task<BaseApiResponse> AddGoods([FromBody]AddGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));

            //创建商品
            var goodsId = GuidUtil.NewSequentialId();
            var   command = new CreateGoodsCommand(
                goodsId,
                request.StoreId,
                request.CategoryIds,
                request.Name,
                request.Description,
                request.Pics,
                request.Price,
                request.OriginalPrice,
                request.Stock,
                request.IsPayOnDelivery,
                request.IsInvoice,
                request.Is7SalesReturn,
                request.Sort);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //初始化默认规格
            var thumb = "";
            if(request.Pics.Any())
            {
                thumb = request.Pics[0];
            }
            var command2 = new AddSpecificationCommand(
                goodsId,
                "默认规格",
                "默认规格",
                thumb,
                request.Price,
                request.Stock,
                request.OriginalPrice,
                request.OriginalPrice/100M,
                "",
                "");
            var result2 = await ExecuteCommandAsync(command2);
            if (!result2.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new AddGoodsResponse {
                GoodsId=goodsId
            };
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
            var goods = _goodsQueryService.GetGoodsAlias(request.Id);
            if (goods == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该商品" };
            }
            //删除
            var command = new DeleteGoodsCommand(request.Id);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "删除失败{0}，可能商品存在预定，无法删除".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 店铺更新商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("StoreUpdateGoods")]
        public async Task<BaseApiResponse> StoreUpdateGoods([FromBody]AddGoodsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var  command = new StoreUpdateGoodsCommand(
                request.CategoryIds,
                request.Name,
                request.Description,
                request.Pics,
                request.Price,
                request.OriginalPrice,
                request.Stock,
                request.IsPayOnDelivery,
                request.IsInvoice,
                request.Is7SalesReturn,
                request.Sort)
            {
                AggregateRootId = request.Id
            };
            
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //更新默认规格
            var specification = _goodsQueryService.GetGoodsDefaultSpecification(request.Id);
            if (specification != null)
            {
                var thumb = specification.Thumb;
                if (request.Pics.Any())
                {
                    thumb = request.Pics[0];
                }
                //更新默认规格
                var command2 = new UpdateSpecificationCommand(
                    request.Id,
                    specification.Id,
                    "默认规格",
                    "默认规格",
                    thumb,
                    request.Price,
                    request.OriginalPrice,
                    request.OriginalPrice/100M,
                    "",
                    "",
                    request.Stock);
                var result2 = await ExecuteCommandAsync(command2);
                if (!result2.IsSuccess())
                {
                    return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
                }
            }
            return new BaseApiResponse();
        }

        
        
        /// <summary>
        /// 获取商品的参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("GetParams")]
        public BaseApiResponse GetParams([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var goodsParams = _goodsQueryService.GetGoodsParams(request.Id);
            if (!goodsParams.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "没有参数" };
            }
            return new GetParamsResponse
            {
                Params = goodsParams.Select(x => new GoodsParam
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value
                }).ToList()
            };
        }

        /// <summary>
        /// 获取规格
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("GetSpecifications")]
        public BaseApiResponse StoreGetSpecifications([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));

            var specifications = _goodsQueryService.GetPublishedSpecifications(request.Id);
            if (!specifications.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "没有数据" };
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
                    Thumb = x.Thumb
                }).ToList()
            };
        }

        /// <summary>
        /// 更新规格
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UpdateGoodsSpecifications")]
        public async Task<BaseApiResponse> UpdateGoodsSpecifications([FromBody]UpdateGoodsSpecificationsRequest request)
        {
            request.CheckNotNull(nameof(request));

            //更新规格
            var command = new UpdateSpecificationsCommand(
                request.GoodsId,
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
                false);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 更新商品参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UpdateGoodsParams")]
        public async Task<BaseApiResponse> UpdateGoodsParams([FromBody]UpdateGoodsParamsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateParamsCommand(request.GoodsId,
                request.Params.Select(x => new Commands.Goodses.GoodsParamInfo(x.Name, x.Value)).ToList());

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 添加商品规格
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddGoodsSpecifications")]
        public async Task<BaseApiResponse> AddGoodsSpecifications([FromBody]AddGoodsSpecificationsRequest request)
        {
            request.CheckNotNull(nameof(request));
            //获取商品，现在商品的规格图片为商品的第一张图片
            var goodsAlis = _goodsQueryService.GetGoodsAlias(request.GoodsId);
            if(goodsAlis==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该商品" };
            }
            var thumbs = goodsAlis.Pics.Split("|", true);
            var thumb = "";
            if(thumbs.Any())
            {
                thumb = thumbs[0];
            }
            var command = new AddSpecificationsCommand(request.GoodsId,
                request.Specifications.Select(x => new Commands.Goodses.Specifications.SpecificationInfo(
                    GuidUtil.NewSequentialId(),
                    x.Name.ExpandAndToString(),
                    x.Value.ExpandAndToString(),
                    thumb,
                    x.Price,
                    x.OriginalPrice,
                    0,
                    x.Number,
                    x.BarCode,
                    x.Stock)).ToList());
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 店铺所有商品 管理用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AllGoodses")]
        public BaseApiResponse AllGoodses([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            var storeInfo = _storeQueryService.InfoByUserId(currentAccount.UserId.ToGuid());
            var goodses  = _goodsQueryService.GetStoreGoodses(storeInfo.Id);
            var pageSize = 20;
            var total = goodses.Count();
            //筛选
            if (request.Status != GoodsStatus.All)
            {
                goodses = goodses.Where(x => x.Status == request.Status);
            }
            if (!request.Name.IsNullOrEmpty())
            {
                goodses = goodses.Where(x => x.Name.Contains(request.Name)).OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
                total = goodses.Count();
            }
            total = goodses.Count();

            //分页
            goodses = goodses.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new AllGoodsResponse
            {
                Total = total,
                Goodses = goodses.Select(x => new GoodsDetails
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    Pics = x.Pics.Split("|", true).ToList(),
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
                    Sort = x.Sort,
                    IsPublished=x.IsPublished,
                    Status = x.Status.ToString(),
                    RefusedReason=x.RefusedReason
                }).ToList()
            };
        }
        #endregion
        
    }
}