using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Request.Orders;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Orders;
using Shop.Apis.Extensions;
using Shop.Apis.Services;
using Shop.Commands.Orders;
using Shop.Commands.Payments;
using Shop.Common.Enums;
using Shop.QueryServices;
using Shop.QueryServices.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;
using Xia.Common.Utils;

namespace Shop.Apis.Controllers
{
    /// <summary>
    /// 预订单 提交等待后流程就结束了，剩下的是商家订单流程
    /// </summary>
    [Route("[controller]")]
    public class OrderController:BaseApiController
    {
        private static readonly TimeSpan PricedOrderWaitTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan PricedOrderPollInterval = TimeSpan.FromMilliseconds(750);

        private IOrderQueryService _orderQueryService;//Q 端

        public OrderController(ICommandService commandService, IContextService contextService,
            IOrderQueryService orderQueryService) : base(commandService,contextService)
        {
            _orderQueryService = orderQueryService;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddOrderRequest request)
        {
            request.CheckNotNull(nameof(request));
            request.ExpressAddress.CheckNotNull(nameof(request.ExpressAddress));

            var currentAccount = _contextService.GetCurrentAccount(HttpContext);

            var command = new PlaceOrderCommand(
                GuidUtil.NewSequentialId(),
                currentAccount.UserId.ToGuid(),
                new Common.ExpressAddressInfo(
                    request.ExpressAddress.Region,
                    request.ExpressAddress.Address,
                    request.ExpressAddress.Name,
                    request.ExpressAddress.Mobile,
                    request.ExpressAddress.Zip
                    ),
                request.CartGoodses.Select(x => new SpecificationInfo(
                    x.SpecificationId,
                    x.GoodsId,
                    x.StoreId,
                    x.GoodsName,
                    x.GoodsPic,
                    x.SpecificationName,
                    x.Price,
                    x.OriginalPrice,
                    x.Quantity,
                    x.Benevolence
                    )).ToList());
            if (!command.Specifications.Any())
            {
                return new BaseApiResponse { Code = 400, Message = "订单至少包含一个商品" };
            }
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //等待订单预定成功
            var order = WaitUntilReservationCompleted(command.AggregateRootId).Result;
            if (order == null)
            {
                return new BaseApiResponse { Code = 400, Message = "商品预定失败，请稍后再试" };
            }

            //预定成功，处理付款信息，要求用户付款
            if (order.Status == (int)OrderStatus.ReservationSuccess)
            {
                return await StartPayment(order.OrderId);
            }
            else
            {
                return new BaseApiResponse { Code = 400, Message = "预定失败，请稍后再试" };
            }
            
        }
        

        #region 私有方法
        /// <summary>
        /// 开启订单支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private async Task<BaseApiResponse> StartPayment(Guid orderId)
        {
            var order = _orderQueryService.FindOrder(orderId);

            if (order == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有该预定单" };
            }
            if (order.Status == (int)OrderStatus.PaymentSuccess || order.Status == (int)OrderStatus.Success)
            {
                return new BaseApiResponse { Code = 400, Message = "预定单已经付款或完成" };
            }
            if (order.ReservationExpirationDate.HasValue && order.ReservationExpirationDate < DateTime.Now)
            {
                return new BaseApiResponse { Code = 400, Message = "预订单已经过期，请重新预定" };
            }
            //要通过支付完成预订单
            return await CompleteRegistrationWithThirdPartyProcessorPayment(order);
        }
        /// <summary>轮训订单状态，直到订单的库存预扣操作完成
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private Task<Order> WaitUntilReservationCompleted(Guid orderId)
        {
            return TimerTaskFactory.StartNew<Order>(
                    () => _orderQueryService.FindOrder(orderId),
                    x => x != null && (x.Status == (int)OrderStatus.ReservationSuccess || x.Status == (int)OrderStatus.ReservationFailed),
                    PricedOrderPollInterval,
                    PricedOrderWaitTimeout);
        }

        //完成预定切未付款
        private async Task<BaseApiResponse> CompleteRegistrationWithoutPayment(Guid orderId,Guid goodsId)
        {
            var command= new MarkAsSuccessCommand(orderId,goodsId);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "创建付款信息失败：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }
        /// <summary>
        /// 创建付款信息，要求前端付款
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task<BaseApiResponse> CompleteRegistrationWithThirdPartyProcessorPayment(Order order)
        {
            //创建付款信息
            var paymentCommand = CreatePaymentCommand(order);

            var result = await ExecuteCommandAsync(paymentCommand);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "创建付款信息失败：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //返回订单ID，付款ID，要求前端进入付款页
            return new AddOrderResponse
            {
                OrderId = order.OrderId,
                PaymentId=paymentCommand.AggregateRootId
            };
        }

        /// <summary>
        /// 创建订单的付款信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private CreatePaymentCommand CreatePaymentCommand(Order order)
        {
            var description = "支付预订单 " + order.OrderId;
            var paymentCommand = new CreatePaymentCommand
            (
                GuidUtil.NewSequentialId(),
                order.OrderId,
                description,
                order.Total,
                order.Lines.Select(x => new PaymentLine {
                    Id = x.SpecificationId,
                    Description = x.SpecificationName,
                    Amount = x.LineTotal })
            );

            return paymentCommand;
        }

        
        #endregion
    }
}