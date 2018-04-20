using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Goodses.Specifications;
using Shop.Commands.Orders;
using Shop.Commands.Stores;
using Shop.Commands.Stores.StoreOrders;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Goodses.Specifications;
using Shop.Domain.Events.Orders;
using Shop.Domain.Events.Payments;
using Shop.Domain.Models.PublishableExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Buy.ProcessManagers
{
    [Component]
    public class OrderProcessManager :
        IMessageHandler<OrderPlacedEvent>,                           //订单创建(Order)
        IMessageHandler<SpecificationReservedEvent>,                  //预定成功(Goods)
        IMessageHandler<SpecificationInsufficientException>,               //预定失败(Goods)
        IMessageHandler<PaymentCompletedEvent>,               //支付成功(Payment)
        IMessageHandler<PaymentRejectedEvent>,                //支付拒绝(Payment)
        IMessageHandler<OrderPaymentConfirmedEvent>,                 //确认支付(Order)
        IMessageHandler<SpecificationReservationCommittedEvent>,      //确认预定(Goods)
        IMessageHandler<SpecificationReservationCancelledEvent>,      //取消预定(Goods)
        IMessageHandler<OrderSuccessedEvent>,                        //订单成功(Order)
        IMessageHandler<OrderExpiredEvent>                        //订单过期时(30分钟过期)(Order)
    {
        private ICommandService _commandService;

        public OrderProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        /// <summary>
        /// 订单预定  发起到所有商品规格的预定命令
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public  Task<AsyncTaskResult> HandleAsync(OrderPlacedEvent evnt)
        {
            var tasks = new List<Task>();
            //通过商品ID 分组
            var goodsGroup = evnt.OrderTotal.Lines.GroupBy(x => x.SpecificationQuantity.Specification.GoodsId);
            foreach(var goods in goodsGroup)
            {
                //一个商品可以有多个规格的预定
                tasks.Add(_commandService.SendAsync(new MakeSpecificationReservationCommand(
                    goods.Key,
                     evnt.AggregateRootId,//订单ID
                                          //查找当前商品的所有规格
                    goods.Select(x => new SpecificationReservationItemInfo(
                            x.SpecificationQuantity.Specification.SpecificationId,
                            x.SpecificationQuantity.Quantity)).ToList()//所有规格和数量
                )));
            }
            
            //执行所以的任务  
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }
        
        /// <summary>
        /// 某个商品发来的预定成功消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservedEvent evnt)
        {
            //发送单个商品预定成功指令给订单 订单处理是否全部预定成功
            return _commandService.SendAsync(new ConfirmOneReservationCommand( evnt.ReservationId,evnt.AggregateRootId, true));
        }
        /// <summary>
        /// 某个商品发来的预定库存不足的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(SpecificationInsufficientException exception)
        {
            return _commandService.SendAsync(new ConfirmOneReservationCommand(exception.ReservationId, exception.GoodsId, false));
        }

        public Task<AsyncTaskResult> HandleAsync(PaymentCompletedEvent evnt)
        {
            return _commandService.SendAsync(new ConfirmPaymentCommand(
                evnt.OrderId, 
                evnt.PayInfo,
                true));
        }
        public Task<AsyncTaskResult> HandleAsync(PaymentRejectedEvent evnt)
        {
            return _commandService.SendAsync(new ConfirmPaymentCommand(evnt.OrderId,
                new PayInfo(0,0), false));
        }

        /// <summary>
        /// 订单付款确认信息 提交商品规格预定
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(OrderPaymentConfirmedEvent evnt)
        {
            var tasks = new List<Task>();
            //通过商品ID 分组
            var goodsGroup = evnt.OrderTotal.Lines.GroupBy(x => x.SpecificationQuantity.Specification.GoodsId);
            foreach (var goods in goodsGroup)
            {
                //每个商品都发送确认预定信息
                if (evnt.OrderStatus == OrderStatus.PaymentSuccess)
                {
                    tasks.Add(_commandService.SendAsync(new CommitSpecificationReservationCommand(
                        goods.Key, 
                        evnt.AggregateRootId)));
                }
                else if (evnt.OrderStatus == OrderStatus.PaymentRejected)
                {
                    tasks.Add(_commandService.SendAsync(new CancelSpecificationReservationCommand(
                        goods.Key,
                        evnt.AggregateRootId)));
                }
            }
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }

        /// <summary>
        /// 商品规格预定确认返回 因为已经预定，所有确认一定会成功 每个商品返回都会发送一个成功命令
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservationCommittedEvent evnt)
        {
            return _commandService.SendAsync(new MarkAsSuccessCommand(evnt.ReservationId,evnt.AggregateRootId));
        }
        /// <summary>
        /// 每个商品都会发送一个取消命令
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(SpecificationReservationCancelledEvent evnt)
        {
            return _commandService.SendAsync(new CloseOrderCommand(evnt.ReservationId, evnt.AggregateRootId));
        }

        /// <summary>
        /// 订单付款成功  分单到商家
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(OrderSuccessedEvent evnt)
        {
            //如果付款成功分单到商家
            if(evnt.PayInfo.Total>0)
            {
                var tasks = new List<Task>();
                //商品按商家ID 分组
                var groupInfo = evnt.OrderTotal.Lines.GroupBy(m => m.SpecificationQuantity.Specification.StoreId).ToList();
                foreach (var item in groupInfo)
                {
                    //分单到商家
                    var storeId = item.Key;
                    var goods = item.ToList();
                    //商家订单的付款详情计算
                    var storeOrderPayDetailInfo = CalculateStoreOrderPayDetail(
                            evnt.PayInfo,
                            goods.Sum(x => x.LineTotal),
                            goods.Sum(x => x.StoreLineTotal));

                    tasks.Add(_commandService.SendAsync(new CreateStoreOrderCommand(
                        GuidUtil.NewSequentialId(),
                        evnt.UserId,
                        storeId,
                        evnt.AggregateRootId,
                        DateTime.Now.ToSerialNumber(),
                        "",//订单备注
                        evnt.ExpressAddressInfo,
                        evnt.PayInfo,//这是Order 的付款信息，不一定全是该商家的
                        storeOrderPayDetailInfo,//这里放置该商家订单付款信息，根据订单付款信息计算
                        goods.Select(x=>new OrderGoods (
                            x.SpecificationQuantity.Specification.GoodsId,
                            x.SpecificationQuantity.Specification.SpecificationId,
                            x.SpecificationQuantity.Specification.GoodsName,
                            x.SpecificationQuantity.Specification.GoodsPic,
                            x.SpecificationQuantity.Specification.SpecificationName,
                            x.SpecificationQuantity.Specification.Price,
                            x.SpecificationQuantity.Specification.OriginalPrice,
                            x.SpecificationQuantity.Quantity,
                            CalculateOrderGoodsPayDetail(
                                storeOrderPayDetailInfo,
                                x.LineTotal,
                                x.StoreLineTotal),//这里是商品的付款详情，根据商家订单计算
                            x.SpecificationQuantity.Specification.Benevolence
                        )).ToList())));
                }
                Task.WaitAll(tasks.ToArray());
            }
            return Task.FromResult(AsyncTaskResult.Success);
        }

        #region 付款详情计算
        /// <summary>
        /// 计算商家订单的付款详情
        /// </summary>
        /// <param name="payInfo">订单的付款信息</param>
        /// <param name="storeOrderTotal">商家订单总额</param>
        private PayDetailInfo CalculateStoreOrderPayDetail(PayInfo payInfo,decimal storeOrderTotal,decimal storeTotal)
        {
            //计算分配到商家订单的购物券金额
            
            var shopCash = Math.Round((storeOrderTotal / payInfo.Total) * payInfo.ShopCash,2);
            return new PayDetailInfo(storeOrderTotal, storeTotal, shopCash);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeOrderPayDetailInfo">商家订单的付款详情</param>
        /// <param name="total">订单商品总额</param>
        /// <param name="storeTotal">订单商品商家供应金额</param>
        /// <returns></returns>
        private PayDetailInfo CalculateOrderGoodsPayDetail(PayDetailInfo storeOrderPayDetailInfo,decimal total,decimal storeTotal)
        {
            var shopCash=Math.Round((total/ storeOrderPayDetailInfo.Total)*storeOrderPayDetailInfo.ShopCash,2);
            return new PayDetailInfo(total, storeTotal, shopCash);
        }
        #endregion


        /// <summary>
        /// 订单过期
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(OrderExpiredEvent evnt)
        {
            var tasks = new List<Task>();
            foreach (var goodsLine in evnt.OrderTotal.Lines)
            {
                //发送给所有商品 取消预定
                tasks.Add( _commandService.SendAsync(
                    new CancelSpecificationReservationCommand(
                        goodsLine.SpecificationQuantity.Specification.GoodsId, 
                        evnt.AggregateRootId)));
            }
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }

        
    }
}
