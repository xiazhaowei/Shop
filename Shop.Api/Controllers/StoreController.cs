using ENode.Commanding;
using Shop.Api.Extensions;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Store;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Goodses;
using Shop.Api.Models.Response.Store;
using Shop.Api.Services;
using Shop.Commands.Stores;
using Shop.Common.Enums;
using Shop.QueryServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class StoreController:BaseApiController
    {
        private IUserQueryService _userQueryService;
        private IStoreQueryService _storeQueryService;//Q端
        private IGoodsQueryService _goodsQueryService;
        private IStoreOrderQueryService _storeOrderQueryService;
        
        public StoreController(ICommandService commandService, IContextService contextService,
            IUserQueryService userQueryService,
            IStoreQueryService storeQueryService, 
            IStoreOrderQueryService storeOrderQueryService,
            IGoodsQueryService goodsQueryService) : base(commandService,contextService)
        {
            _userQueryService = userQueryService;
            _storeQueryService = storeQueryService;
            _storeOrderQueryService = storeOrderQueryService;
            _goodsQueryService = goodsQueryService;
        }
        /// <summary>
        /// 我的店铺信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse Info()
        {
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var storeInfo = _storeQueryService.InfoByUserId(currentAccount.UserId.ToGuid());
            if (storeInfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有店铺" };
            }
            //获取未发货订单
            var placedStoreOrderes = _storeOrderQueryService.StoreStoreOrderDetails(storeInfo.Id, StoreOrderStatus.Placed);
            
            return new StoreInfoResponse
            {
                StoreInfo = new StoreInfo
                {
                    Id = storeInfo.Id,
                    AccessCode = storeInfo.AccessCode,
                    Name = storeInfo.Name,
                    Description = storeInfo.Description,
                    Region = storeInfo.Region,
                    Address = storeInfo.Address,
                    Type = storeInfo.Type.ToDescription(),
                    Status = storeInfo.Status.ToDescription()
                },
                SubjectInfo = new SubjectInfo
                {
                    SubjectName = storeInfo.SubjectName,
                    SubjectNumber = storeInfo.SubjectNumber,
                    SubjectPic = storeInfo.SubjectPic
                },
                StatisticsInfo = new StatisticsInfo
                {
                    TodaySale = storeInfo.TodaySale,
                    TodayOrder = storeInfo.TodayOrder,
                    TotalSale = storeInfo.TotalSale,
                    TotalOrder = storeInfo.TotalOrder
                },
                StoreOrders = placedStoreOrderes.Select(x => new Api.Models.Response.StoreOrders.StoreOrder
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    Region = x.Region,
                    Number = x.Number,
                    Remark = x.Remark,
                    ExpressAddress = x.ExpressAddress,
                    ExpressRegion = x.ExpressRegion,
                    ExpressMobile = x.ExpressMobile,
                    ExpressName = x.ExpressName,
                    ExpressZip = x.ExpressZip,
                    CreatedOn = x.CreatedOn,
                    Total = x.Total,
                    StoreTotal = x.StoreTotal,
                    Status = x.Status.ToDescription(),
                    StoreOrderGoodses = x.StoreOrderGoodses.Select(z => new Api.Models.Response.StoreOrders.StoreOrderGoods
                    {
                        Id = z.Id,
                        GoodsId = z.GoodsId,
                        SpecificationId = z.SpecificationId,
                        SpecificationName = z.SpecificationName,
                        GoodsName = z.GoodsName,
                        GoodsPic = z.GoodsPic,
                        Quantity = z.Quantity,
                        Price = z.Price,
                        OriginalPrice = z.OriginalPrice,
                        Total = z.Total,
                        StoreTotal = z.StoreTotal,
                    }).ToList()
                }).ToList()
            };
            
            

        }

        /// <summary>
        /// 店铺退货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse ReturnAddressInfo(ReturnAddressInfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var storeInfo = _storeQueryService.Info(request.Id);
            if (storeInfo == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有店铺" };
            }

            return new ReturnAddressInfoResponse
            {
                ReturnAddressInfo = new ReturnAddressInfo
                {
                    StoreId = storeInfo.Id,
                    ReturnAddress = storeInfo.ReturnAddress,
                    ReturnAddressMobile = storeInfo.ReturnAddressMobile,
                    ReturnAddressName = storeInfo.ReturnAddressName
                }
            };
        }

        /// <summary>
        /// 店铺信息首页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseApiResponse HomeInfo(InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var storeInfo = _storeQueryService.Info(request.Id  );
            if (storeInfo == null)
            {
                return new BaseApiResponse();
            }
            var goodses = _goodsQueryService.GetStoreGoodses(request.Id ).Where(x=>x.IsPublished && x.Status==GoodsStatus.Verifyed).OrderByDescending(x=>x.CreatedOn).Take(60);
            if (storeInfo != null)
            {
                return new HomeInfoResponse
                {
                    StoreInfo = new StoreInfo
                    {
                        Id = storeInfo.Id,
                        AccessCode = storeInfo.AccessCode,
                        Name = storeInfo.Name,
                        Description = storeInfo.Description,
                        Region = storeInfo.Region,
                        Address = storeInfo.Address,
                        Type = storeInfo.Type.ToDescription(),
                        Status = storeInfo.Status.ToDescription()
                    },
                    SubjectInfo=new SubjectInfo
                    {
                        SubjectName=storeInfo.SubjectName,
                        SubjectNumber=storeInfo.SubjectNumber,
                        SubjectPic=storeInfo.SubjectPic
                    },
                    Goodses = goodses.Select(x => new Goods
                    {
                        Id = x.Id,
                        Pics= x.Pics.Split("|", true).Select(img => img.ToOssStyleUrl(OssImageStyles.GoodsMainPic.ToDescription())).ToList(),
                        Name = x.Name,
                        Price = x.Price,
                        OriginalPrice=x.OriginalPrice,
                        Benevolence =x.Benevolence,
                        SellOut=x.SellOut
                    }).ToList()
                };
            }
            else
            {
                return new BaseApiResponse { Code = 400, Message = "没找到店铺" };
            }

        }

        #region 登录 创建

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ApplyStore(ApplyStoreRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext.Current);

            var userInfo = _userQueryService.FindUser(currentAccount.UserId.ToGuid());
            if (userInfo.Role == UserRole.Consumer)
            {
                return new BaseApiResponse { Code = 400, Message = "Vip以上会员才可以申请开店" };
            }

            var store = _storeQueryService.InfoByUserId(currentAccount.UserId.ToGuid());
            if (store != null)
            {
                return new BaseApiResponse { Code = 400, Message = "您已开店，无法继续开店" };
            }

            var command = new CreateStoreCommand(
                GuidUtil.NewSequentialId(),
                currentAccount.UserId.ToGuid(),
                request.AccessCode,
                request.Name,
                request.Description,
                request.Region,
                request.Address,
                request.Subject.Name,
                request.Subject.Number,
                request.Subject.Pic);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 设置管理密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetAccessCode(SetAccessCodeRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new SetAccessCodeCommand(request.Id, request.AccessCode);

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        

        #endregion

        #region 店铺设置
        /// <summary>
        /// 编辑基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CustomerUpdateStoreCommand(
                request.Name,
                request.Description,
                request.Address)
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
        /// 更新退货地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> EditReturnAddress(EditReturnAddressRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateReturnAddressCommand(
                request.Name,
                request.Mobile,
                request.Address)
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
        /// 编辑主体信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> EditSubject(EditSubjectRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UpdateSubjectCommand(
                request.SubjectName,
                request.SubjectNumber,
                request.SubjectPic)
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