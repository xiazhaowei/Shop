using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Users;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Domain.Events.OfflineStores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.ProcessManagers
{
    [Component]
    public class OfflineStoreProcessManager :
        IMessageHandler<NewSaleAcceptedEvent>  //线下店铺接受了新的消费
    {
        private ICommandService _commandService;

        public OfflineStoreProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
        
        /// <summary>
        /// 店铺接受新的销售
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(NewSaleAcceptedEvent evnt)
        {
            //发送两个命令 一个到店铺所有者接受店铺销售，一个到消费者得到福豆
            var tasks = new List<Task>();

            //发送给商家所有人付款的指令
            tasks.Add(_commandService.SendAsync(new CreateCashTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.StoreOwnerWalletId,
                DateTime.Now.ToSerialNumber(),
                Common.Enums.CashTransferType.StoreSell,
                Common.Enums.CashTransferStatus.Placed,
                evnt.StoreAmount,
                0,
                Common.Enums.WalletDirection.In,
                "线下消费"
                )));
            //发送给用户获取福豆奖励
            tasks.Add(_commandService.SendAsync(new CreateBenevolenceTransferCommand(
                GuidUtil.NewSequentialId(),
                evnt.UserWalletId,
                DateTime.Now.ToSerialNumber(),
                Common.Enums.BenevolenceTransferType.ShoppingAward,
                Common.Enums.BenevolenceTransferStatus.Placed,
                evnt.BenevolenceAmount,
                0,
                Common.Enums.WalletDirection.In,
                "线下消费奖励"
                )));
            //消费者的推荐人获取一度二度奖励
            tasks.Add(_commandService.SendAsync(new MyParentCanGetBenevolenceCommand(evnt.BenevolenceAmount) {
                AggregateRootId=evnt.UserId
            }));
            //店铺推荐者获取收益
            tasks.Add(_commandService.SendAsync(new AcceptMyStoreNewSaleCommand(evnt.StoreOwnerWalletId, evnt.Amount) {
                AggregateRootId=evnt.StoreOwnerId
            }));

            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }
    }
}
