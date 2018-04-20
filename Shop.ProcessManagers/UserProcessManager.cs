using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Carts;
using Shop.Commands.Users;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.ProcessManagers
{
    [Component]
    public class UserProcessManager :
        IMessageHandler<UserCreatedEvent>,//创建用户
        IMessageHandler<UserFreezeEvent>,//冻结用户
        IMessageHandler<UserUnFreezeEvent>,//用户解冻
        IMessageHandler<UserSpendingTransformToBenevolenceEvent>,//用户消费转换为善心(User)
        IMessageHandler<UserGetSaleBenevolenceEvent>,//用户店铺销售激励善心
        IMessageHandler<MyParentCanGetBenevolenceEvent>,//我的父亲可以获得推荐善心奖励
        IMessageHandler<UserGetChildBenevolenceEvent>,//获取子的善心分成
        IMessageHandler<UserGetChildCashEvent>,
        IMessageHandler<UserGetChildProfitBenevolenceEvent>,//获取子的领导奖金
        IMessageHandler<UserGetChildStoreSaleBenevolenceEvent>,//获取子商家销售分成 善心形式
        IMessageHandler<UserGetChildStoreSaleCashEvent>,//获取子商家销售分成 现金生成
        IMessageHandler<AcceptedChildStoreSaleBenevolenceEvent>,
        IMessageHandler<AcceptedChildStoreSaleCashEvent>,
        IMessageHandler<MyParentRecommandAPasserEvent>,
        IMessageHandler<UserDirectGetRecommandVipAwardEvent>,//获取直推VIP奖金
        IMessageHandler<UserGetRecommandVipAwardEvent>,//获取推荐VIP奖金
        IMessageHandler<MyParentCanGetGratefulAwardEvent>,//我的推荐人可以获取感恩奖金
        IMessageHandler<UserGetChildGratefulAwardEvent>//获取感恩奖金
    {
        private ICommandService _commandService;

        public UserProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
       
        /// <summary>
        /// 用户消费额转换为善心 用户剩余未转换逻辑已经实现，这里只需增加用户的善心就行了
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserSpendingTransformToBenevolenceEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateBenevolenceTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    BenevolenceTransferType.ShoppingAward,
                    BenevolenceTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "购物激励"));
        }

      
        public Task<AsyncTaskResult> HandleAsync(UserGetSaleBenevolenceEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateBenevolenceTransferCommand(
                    GuidUtil.NewSequentialId(),     
                    evnt.WalletId,
                    number,
                    BenevolenceTransferType.StoreAward,
                    BenevolenceTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "店铺销售奖励"));
        }
        /// <summary>
        /// 我的父亲可以获得推荐善心奖励,递归
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(MyParentCanGetBenevolenceEvent evnt)
        {
            return _commandService.SendAsync(
                new AcceptChildBenevolenceCommand(evnt.ParentId,evnt.Amount,evnt.ProfitAmount,evnt.HighProfitAmount,evnt.Level));
        }
        /// <summary>
        /// 获取 子的善心分成
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserGetChildBenevolenceEvent evnt)
        {
            //推荐奖励
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateBenevolenceTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.WalletId,
                number,
                BenevolenceTransferType.RecommendUserAward,
                BenevolenceTransferStatus.Placed,
                evnt.Amount,
                0,
                WalletDirection.In,
                "推荐用户{0}度激励".FormatWith(evnt.Level)));
        }
        public Task<AsyncTaskResult> HandleAsync(UserGetChildProfitBenevolenceEvent evnt)
        {
            //领导奖
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateBenevolenceTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.WalletId,
                number,
                BenevolenceTransferType.RecommendUserAward,
                BenevolenceTransferStatus.Placed,
                evnt.Amount,
                0,
                WalletDirection.In,
                "领导奖励"));
        }
        /// <summary>
        /// 推荐商家售货奖励
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserGetChildStoreSaleBenevolenceEvent evnt)
        {
            return _commandService.SendAsync(new GetChildStoreSaleBenevolenceCommand(evnt.ParentId,evnt.Amount));
        }

        public Task<AsyncTaskResult> HandleAsync(UserGetChildStoreSaleCashEvent evnt)
        {
            return _commandService.SendAsync(new GetChildStoreSaleCashCommand(evnt.ParentId, evnt.Amount));
        }

        /// <summary>
        /// 创建用户顺便创建用户的钱包信息
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(UserCreatedEvent evnt)
        {
            var tasks = new List<Task>();
            var number = DateTime.Now.ToSerialNumber();
            
            //创建用户的钱包信息
            tasks.Add( _commandService.SendAsync(new CreateWalletCommand(evnt.WalletId,
                evnt.AggregateRootId)));
            //创建用户的购物车信息
            tasks.Add(_commandService.SendAsync(new CreateCartCommand(evnt.CartId,
                evnt.AggregateRootId)));

            if (evnt.ParentId != Guid.Empty)
            {
                //给用户的推荐人发送成功邀请的命令
                tasks.Add(_commandService.SendAsync(new InvotedNewUserCommand(evnt.AggregateRootId)
                {
                    AggregateRootId = evnt.ParentId
                }));
            }

            //给用户购物券-暂停
            //tasks.Add(_commandService.SendAsync(new CreateShopCashTransferCommand(
            //        GuidUtil.NewSequentialId(),
            //        evnt.WalletId,
            //        number,
            //        ShopCashTransferType.SystemOp,
            //        ShopCashTransferStatus.Placed,
            //        RandomArray.NewUserShopCash(),
            //        0,
            //        WalletDirection.In,
            //        "新用户购物券奖励")));
            //执行所以的任务    
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }

        public Task<AsyncTaskResult> HandleAsync(AcceptedChildStoreSaleBenevolenceEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();

            return _commandService.SendAsync(new CreateBenevolenceTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    BenevolenceTransferType.RecommendStoreAward,
                    BenevolenceTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "推荐商家售货激励"));
        }
        public Task<AsyncTaskResult> HandleAsync(AcceptedChildStoreSaleCashEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();

            return _commandService.SendAsync(new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    CashTransferType.RecommendStoreAward,
                    CashTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "推荐商家售货奖励"));
        }

        public Task<AsyncTaskResult> HandleAsync(UserFreezeEvent evnt)
        {
            //冻结用户的钱包
            return _commandService.SendAsync(new FreezeWalletCommand {
                AggregateRootId=evnt.WalletId
            });
        }

        public Task<AsyncTaskResult> HandleAsync(UserUnFreezeEvent evnt)
        {
            //解冻用户的钱包
            return _commandService.SendAsync(new UnFreezeWalletCommand
            {
                AggregateRootId = evnt.WalletId
            });
        }

        public Task<AsyncTaskResult> HandleAsync(MyParentRecommandAPasserEvent evnt)
        {
            if (evnt.ParentId != Guid.Empty)
            {
                return _commandService.SendAsync(
                new AcceptChildUpdateOrderCommand(evnt.NewVipId, evnt.NewVipRole,
                    evnt.GoodsCount,evnt.LeftAwardAmount, evnt.Level,evnt.UpdateOrderType)
                {
                    AggregateRootId = evnt.ParentId
                });
            }
            return Task.FromResult(AsyncTaskResult.Success);
        }

        public Task<AsyncTaskResult> HandleAsync(UserDirectGetRecommandVipAwardEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            //给用户直推奖励
            return _commandService.SendAsync(new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    CashTransferType.RecommendUserAward,
                    CashTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "直推用户奖励"));
        }

        public Task<AsyncTaskResult> HandleAsync(UserGetRecommandVipAwardEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            //给用户间接推荐奖励
            return _commandService.SendAsync(new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.WalletId,
                    number,
                    CashTransferType.RecommendUserAward,
                    CashTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "经理/总监奖励"));
        }

        //我的推荐人可以获取感恩奖
        public Task<AsyncTaskResult> HandleAsync(MyParentCanGetGratefulAwardEvent evnt)
        {
            return _commandService.SendAsync(
                new AcceptChildGratefulAwardCommand(evnt.ParentId, evnt.Amount,evnt.Remak));
        }

        public Task<AsyncTaskResult> HandleAsync(UserGetChildGratefulAwardEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.WalletId,
                number,
                CashTransferType.RecommendUserAward,
                CashTransferStatus.Placed,
                evnt.Amount,
                0,
                WalletDirection.In,
                "感恩奖："+evnt.Remark));
        }

        public Task<AsyncTaskResult> HandleAsync(UserGetChildCashEvent evnt)
        {
            //推荐奖励
            var number = DateTime.Now.ToSerialNumber();
            return _commandService.SendAsync(new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.WalletId,
                number,
                CashTransferType.RecommendUserAward,
                CashTransferStatus.Placed,
                evnt.Amount,
                0,
                WalletDirection.In,
                "推荐用户{0}度激励".FormatWith(evnt.Level)));
        }
    }
}
