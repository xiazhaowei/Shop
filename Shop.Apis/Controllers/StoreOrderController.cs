using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.StoreOrders;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.StoreOrders;
using Shop.Api.Utils;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Commands.Stores.StoreOrders;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class StoreOrderController:BaseApiController
    {

        private IStoreOrderQueryService _storeOrderQueryService;//Q 端

        public StoreOrderController(ICommandService commandService, IContextService contextService,
            IStoreOrderQueryService storeOrderQueryService) : base(commandService,contextService)
        {
            _storeOrderQueryService = storeOrderQueryService;
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
            var order = _storeOrderQueryService.Find(request.Id);
            if (order == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该订单" };
            }
            //删除
            var command = new DeleteStoreOrderCommand(request.Id);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "删除失败{0}，可能订单状态不允许删除".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        #region 查询
        /// <summary>
        /// 包裹详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Info")]
        public BaseApiResponse Info([FromBody]InfoRequest request)
        {
            request.CheckNotNull(nameof(request));
            var storeorder = _storeOrderQueryService.FindOrder(request.Id);
            if(storeorder==null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到该订单" };
            }
            return new InfoResponse
            {
                Id = storeorder.Id,
                StoreId = storeorder.StoreId,
                Number = storeorder.Number,
                Region = storeorder.Region,
                Remark = storeorder.Remark,
                ExpressRegion = storeorder.ExpressRegion,
                ExpressAddress = storeorder.ExpressAddress,
                ExpressName = storeorder.ExpressName,
                ExpressMobile = storeorder.ExpressMobile,
                ExpressZip = storeorder.ExpressZip,
                CreatedOn = storeorder.CreatedOn,
                Total = storeorder.Total,
                ShopCash=storeorder.ShopCash,
                Status = storeorder.Status.ToDescription(),
                StoreOrderGoodses = storeorder.StoreOrderGoodses.Select(z => new StoreOrderGoods
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
                    StoreTotal = z.StoreTotal
                }).ToList()
            };
        }

        /// <summary>
        /// 查询订单物流状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ExpressSchedule")]
        public string ExpressSchedule([FromBody]ExpressScheduleRequest request)
        {
            request.CheckNotNull(nameof(request));

            var expressSchedule = ExpressScheduleUtil.GetSchedule(request.ExpressName, request.ExpressNumber);

            return expressSchedule;
        }


        /// <summary>
        /// 用户的订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UserOrders")]
        public BaseApiResponse UserOrders([FromBody]UserOrdersRequest request)
        {
            request.CheckNotNull(nameof(request));
            var currentAccount = _contextService.GetCurrentAccount(HttpContext);
            //获取数据
            int pageSize = 10;
            var storeOrders = _storeOrderQueryService.UserStoreOrderDetails(currentAccount.UserId.ToGuid());
            var total = storeOrders.Count();
            //筛选数据
            if (request.Status!=StoreOrderStatus.All)
            {
                storeOrders = storeOrders.Where(x=>x.Status==request.Status);
            }
            total = storeOrders.Count();
            //分页
            storeOrders = storeOrders.OrderByDescending(x=>x.CreatedOn).Skip(pageSize * (request.Page-1)).Take(pageSize);

            return new UserOrdersResponse
            {
                Total = total,
                StoreOrders = storeOrders.Select(x => new StoreOrder
                {
                    Id = x.Id,
                    StoreId = x.StoreId,
                    Region = x.Region,
                    Number=x.Number,
                    Remark= x.Remark,
                    ExpressAddress = x.ExpressAddress,
                    ExpressRegion = x.ExpressRegion,
                    ExpressMobile = x.ExpressMobile,
                    ExpressName = x.ExpressName,
                    ExpressZip = x.ExpressZip,

                    DeliverExpressName =x.DeliverExpressName,
                    DeliverExpressCode=x.DeliverExpressCode,
                    DeliverExpressNumber=x.DeliverExpressNumber,

                    ReturnDeliverExpressName = x.ReturnDeliverExpressName,
                    ReturnDeliverExpressCode = x.ReturnDeliverExpressCode,
                    ReturnDeliverExpressNumber = x.ReturnDeliverExpressNumber,

                    Reason =x.Reason,
                    RefundAmount=x.RefundAmount.HasValue? x.RefundAmount.Value :0,
                    StoreRemark=x.StoreRemark,

                    CreatedOn = x.CreatedOn,
                    Total = x.Total,
                    ShopCash=x.ShopCash,
                    Status = x.Status.ToDescription(),
                    StoreOrderGoodses=x.StoreOrderGoodses.Select(z=>new StoreOrderGoods {
                        Id=z.Id,
                        GoodsId=z.GoodsId,
                        SpecificationId=z.SpecificationId,
                        SpecificationName=z.SpecificationName,
                        GoodsName=z.GoodsName,
                        GoodsPic=z.GoodsPic,
                        Quantity=z.Quantity,
                        Price=z.Price,
                        Total=z.Total
                    }).ToList()
                }).ToList()
            };
        }

        /// <summary>
        /// 商家的订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("StoreOrders")]
        public BaseApiResponse StoreOrders([FromBody]StoreOrdersRequest request)
        {
            request.CheckNotNull(nameof(request));

            //获取数据
            int pageSize = 10;
            var storeOrders = _storeOrderQueryService.StoreStoreOrderDetails(request.Id);
            var total = storeOrders.Count();
            //筛选数据
            if (request.Status != StoreOrderStatus.All)
            {
                storeOrders = storeOrders.Where(x => x.Status == request.Status);
            }
            //订单号查询
            if (!string.IsNullOrEmpty(request.OrderNumber))
            {
                storeOrders = storeOrders.Where(x => x.Number.Contains(request.OrderNumber));
            }
            total = storeOrders.Count();
            //分页
            storeOrders = storeOrders.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page-1)).Take(pageSize);

            return new UserOrdersResponse
            {
                Total=total,
                StoreOrders = storeOrders.Select(x => new StoreOrder
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
                    ShopCash=x.ShopCash,
                    Status = x.Status.ToDescription(),

                    DeliverExpressName=x.DeliverExpressName,
                    DeliverExpressCode=x.DeliverExpressCode,
                    DeliverExpressNumber=x.DeliverExpressNumber,

                    ReturnDeliverExpressName = x.ReturnDeliverExpressName,
                    ReturnDeliverExpressCode = x.ReturnDeliverExpressCode,
                    ReturnDeliverExpressNumber = x.ReturnDeliverExpressNumber,

                    Reason =x.Reason,
                    RefundAmount=x.RefundAmount.HasValue? x.RefundAmount.Value : 0,
                    StoreRemark=x.StoreRemark,

                    StoreOrderGoodses = x.StoreOrderGoodses.Select(z => new StoreOrderGoods
                    {
                        Id = z.Id,
                        GoodsId = z.GoodsId,
                        SpecificationId = z.SpecificationId,
                        SpecificationName = z.SpecificationName,
                        GoodsName = z.GoodsName,
                        GoodsPic = z.GoodsPic,
                        Quantity = z.Quantity,
                        Price = z.Price,
                        Total = z.Total
                    }).ToList()
                }).ToList()
            };
        }
        #endregion

        #region 包裹服务


        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Deliver")]
        public async Task<BaseApiResponse> Deliver([FromBody]DeliverRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new DeliverCommand(request.ExpressName,
                request.ExpressCode,
                request.ExpressNumber)
            {
                AggregateRootId=request.Id
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            return new BaseApiResponse();
            
        }

        /// <summary>
        /// 填写退货信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ReturnDeliver")]
        public async Task<BaseApiResponse> ReturnDeliver([FromBody]DeliverRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new ReturnDeliverCommand(request.ExpressName,
                request.ExpressCode,
                request.ExpressNumber)
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
        /// 确认收货
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ConfirmDeliver")]
        public async Task<BaseApiResponse> ConfirmDeliver([FromBody]StoreOrderOpRequest request)
        {
            request.CheckNotNull(nameof(request));
            var command = new ConfirmDeliverCommand
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
        /// 申请退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ApplyRefund")]
        public async Task<BaseApiResponse> ApplyRefund([FromBody]ApplyRefundRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new ApplyRefundCommand(request.Reason, request.RefundAmount)
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
        /// 申请退货退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ApplyReturnAndRefund")]
        public async Task<BaseApiResponse> ApplyReturnAndRefund([FromBody]ApplyRefundRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new ApplyReturnAndRefundCommand(request.Reason, request.RefundAmount)
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
        /// 同意退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AgreeRefund")]
        public async Task<BaseApiResponse> AgreeRefund([FromBody]StoreOrderOpRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new AgreeRefundCommand()
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
        /// 同意退货
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AgreeReturn")]
        public async Task<BaseApiResponse> AgreeReturn([FromBody]AgreeReturnRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new AgreeReturnCommand(request.Remark)
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