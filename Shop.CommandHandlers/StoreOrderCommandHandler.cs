using ENode.Commanding;
using Shop.Commands.Stores.StoreOrders;
using Shop.Domain.Models.Stores;
using Shop.Domain.Models.Users;
using System;
using System.Linq;

namespace Shop.CommandHandlers
{
    public class StoreOrderCommandHandler:
        ICommandHandler<CreateStoreOrderCommand>,
        ICommandHandler<DeleteStoreOrderCommand>,
        ICommandHandler<DeliverCommand>,
        ICommandHandler<ReturnDeliverCommand>,
        ICommandHandler<ConfirmDeliverCommand>,
        ICommandHandler<ApplyRefundCommand>,
        ICommandHandler<ApplyReturnAndRefundCommand>,
        ICommandHandler<AgreeRefundCommand>,
        ICommandHandler<AgreeReturnCommand>
    {
        public void Handle(ICommandContext context,CreateStoreOrderCommand command)
        {
            //从上下文中获取商家的地区信息
            var region = context.Get<Store>(command.StoreId).GetInfo().Region;
            //付款者钱包ID
            var walletId = context.Get<User>(command.UserId).GetWalletId();
            var userid = context.Get<Store>(command.StoreId).GetUserId();
            var storeownerwalletid = context.Get<User>(userid).GetWalletId();

            var storeOrder = new StoreOrder(
                    command.AggregateRootId,
                    walletId,
                    storeownerwalletid,
                    new Domain.Models.Stores.StoreOrders.StoreOrderInfo(
                        command.UserId,
                        command.OrderId,
                        command.StoreId,
                        region,
                        command.Number,
                        command.Remark),
                    command.ExpressAddressInfo,
                    command.PayInfo,
                    command.PayDetailInfo,
                    command.OrderGoodses.Select(x => new OrderGoodsInfo(
                        x.GoodsId,
                        x.SpecificationId,
                        walletId,
                        storeownerwalletid,
                        x.GoodsName,
                        x.GoodsPic,
                        x.SpecificationName,
                        x.Price,
                        x.OrigianlPrice,
                        x.Quantity,
                        x.PayDetailInfo,
                        DateTime.Now,
                        x.Benevolence)
                    ).ToList()
                );
            //添加到上下文
            context.Add(storeOrder);
        }

        public void Handle(ICommandContext context, DeliverCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).Deliver(
                new Domain.Models.Stores.StoreOrders.ExpressInfo(
                command.ExpressName,
                command.ExpressCode,
                command.ExpressNumber));
        }
        public void Handle(ICommandContext context, ReturnDeliverCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).ReturnDeliver(
                new Domain.Models.Stores.StoreOrders.ExpressInfo(
                command.ExpressName,
                command.ExpressCode,
                command.ExpressNumber));
        }

        public void Handle(ICommandContext context, ApplyRefundCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).ApplyRefund(new Domain.Models.Stores.StoreOrders.RefundApplyInfo(command.Reason,
                command.RefundAmount));
        }

        public void Handle(ICommandContext context, ApplyReturnAndRefundCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).ApplyReturnAndRefund(new Domain.Models.Stores.StoreOrders.RefundApplyInfo(command.Reason,
                command.RefundAmount));
        }

        public void Handle(ICommandContext context, AgreeRefundCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).AgreeRefund();
        }

        public void Handle(ICommandContext context, AgreeReturnCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).AgreeReturn(command.Remark);
        }

        public void Handle(ICommandContext context, ConfirmDeliverCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).ConfirmExpress();
        }

        public void Handle(ICommandContext context, DeleteStoreOrderCommand command)
        {
            context.Get<StoreOrder>(command.AggregateRootId).Delete();
        }

    }
}
