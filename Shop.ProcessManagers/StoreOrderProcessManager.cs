using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Stores;
using Shop.Commands.Stores.StoreOrders.OrderGoodses;
using Shop.Commands.Users;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Domain.Events.Stores.StoreOrders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.ProcessManagers
{
    [Component]
    public class StoreOrderProcessManager :
        IMessageHandler<StoreOrderCreatedEvent>, //创建商家订单时
        IMessageHandler<StoreOrderConfirmExpressedEvent>,//用户确认收货，给商家结算
        IMessageHandler<AgreeRefundedEvent> //同意包裹退款
    {
        private ICommandService _commandService;

        public StoreOrderProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        

        /// <summary>
        /// 创建商家订单  发送命令创建订单商品
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreOrderCreatedEvent evnt)
        {
            var tasks = new List<Task>();
            int updateGoodsCount = 0;
            int updateVipPasserGoodsCount = 0;
            //遍历订单商品分别发送创建订单商品命令
            foreach (var orderGoods in evnt.OrderGoodses)
            {
                //供货价为0，售价大于365的商品为升级商品
                if(orderGoods.OriginalPrice==0 && orderGoods.Price>=365M)
                {
                    if (orderGoods.Price >= 3650M)
                    {
                        updateVipPasserGoodsCount += orderGoods.Quantity;
                    }
                    else
                    {
                        updateGoodsCount += orderGoods.Quantity;
                    }
                    
                }
                tasks.Add(_commandService.SendAsync(new CreateOrderGoodsCommand(
                        GuidUtil.NewSequentialId(),
                        evnt.Info.StoreId,
                        evnt.AggregateRootId,
                        new OrderGoods(
                            orderGoods.GoodsId,
                            orderGoods.SpecificationId,
                            orderGoods.GoodsName,
                            orderGoods.GoodsPic,
                            orderGoods.SpecificationName,
                            orderGoods.Price,
                            orderGoods.OriginalPrice,
                            orderGoods.Quantity,
                            orderGoods.PayDetailInfo,
                            orderGoods.Benevolence)
                        {
                            WalletId=orderGoods.WalletId,
                            StoreOwnerWalletId=orderGoods.StoreOwnerWalletId
                        })));
            }

            if (updateGoodsCount>0)
            {
                //包含升级VIP产品
                tasks.Add(_commandService.SendAsync(new AcceptNewUpdateOrderCommand(updateGoodsCount,Common.Enums.UpdateOrderType.VipOrder) {
                    AggregateRootId=evnt.Info.UserId
                }));
            }
            if (updateVipPasserGoodsCount > 0)
            {
                //包含升级经理产品
                tasks.Add(_commandService.SendAsync(new AcceptNewUpdateOrderCommand(updateVipPasserGoodsCount, Common.Enums.UpdateOrderType.VipPasserOrder)
                {
                    AggregateRootId = evnt.Info.UserId
                }));
            }

            //发送给商家接受新的销售
            tasks.Add(_commandService.SendAsync(
                new AcceptNewStoreOrderCommand(evnt.Info.StoreId,
                evnt.AggregateRootId)));

            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }

        public Task<AsyncTaskResult> HandleAsync(AgreeRefundedEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            var tasks = new List<Task>();
            //发送退款指令
            if (evnt.CashRefundAmount > 0)
            {
                tasks.Add( _commandService.SendAsync(new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    Common.Enums.CashTransferType.Refund,
                    Common.Enums.CashTransferStatus.Placed,
                    evnt.CashRefundAmount,
                    0,
                    Common.Enums.WalletDirection.In,
                    "订单退款"
                    )));
            }
            if (evnt.ShopCashRefundAmount > 0)
            {
                tasks.Add( _commandService.SendAsync(new CreateShopCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    Common.Enums.ShopCashTransferType.Refund,
                    Common.Enums.ShopCashTransferStatus.Placed,
                    evnt.ShopCashRefundAmount,
                    0,
                    Common.Enums.WalletDirection.In,
                    "订单退款"
                    )));
            }
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }

        

        /// <summary>
        /// 确认收货，系统给商家结算销售收入
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(StoreOrderConfirmExpressedEvent evnt)
        {
            var tasks = new List<Task>();

            //给商家结算货款
            tasks.Add( _commandService.SendAsync(new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.StoreOwnerWalletId,
                DateTime.Now.ToSerialNumber(),
                Common.Enums.CashTransferType.StoreSell,
                Common.Enums.CashTransferStatus.Placed,
                evnt.StoreGetAmount,
                0,
                Common.Enums.WalletDirection.In,
                "店铺销售商品"
                )));

            foreach(var orderGoods in evnt.OrderGoodses)
            {
                var highProfitAmount = 0M;
                //高倍产品金额
                if(orderGoods.PayDetailInfo.StoreTotal *10 <= orderGoods.PayDetailInfo.Total)
                {
                    highProfitAmount = orderGoods.PayDetailInfo.Total;
                }
                //用户者的购物奖励
                tasks.Add(_commandService.SendAsync(new AcceptMyNewSpendingCommand(
                    evnt.WalletId,
                    orderGoods.PayDetailInfo.Total,
                    orderGoods.PayDetailInfo.StoreTotal,
                    orderGoods.Benevolence*orderGoods.Quantity,
                    highProfitAmount
                    )));
                //店铺所有人接受店铺新的销售额
                tasks.Add(_commandService.SendAsync(new AcceptMyStoreNewSaleCommand(
                    evnt.StoreOwnerWalletId,
                    orderGoods.PayDetailInfo.Total
                    )));
            }

            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }
    }
}
