using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Common;
using Shop.Common.Enums;
using Shop.Domain.Events.Wallets;
using Shop.Domain.Events.Wallets.RechargeApplys;
using Shop.Domain.Events.Wallets.WithdrawApplys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xia.Common;
using Xia.Common.Extensions;

namespace Shop.ProcessManagers
{
    [Component]
    public class WalletProcessManager :
        IMessageHandler<WithdrawApplySuccessEvent>,
        IMessageHandler<WithdrawApplyRejectedEvent>,
        IMessageHandler<RechargeApplySuccessEvent>,
        IMessageHandler<IncentiveUserBenevolenceEvent>//激励福豆
    {
        private ICommandService _commandService;

        public WalletProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }


        /// <summary>
        /// 提现申请审核通过，发送一个提现记录
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(WithdrawApplySuccessEvent evnt)
        {
            return _commandService.SendAsync(
                new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.AggregateRootId,
                    DateTime.Now.ToSerialNumber(),
                    CashTransferType.Withdraw,
                    CashTransferStatus.Success,
                    evnt.Amount,
                    0,
                    WalletDirection.Out,
                    "提现申请成功"));
        }

        public Task<AsyncTaskResult> HandleAsync(RechargeApplySuccessEvent evnt)
        {
            return _commandService.SendAsync(
                new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.AggregateRootId,
                    DateTime.Now.ToSerialNumber(),
                    CashTransferType.Charge,
                    CashTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "线下充值申请成功"));
        }

        /// <summary>
        /// 激励用户善心
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(IncentiveUserBenevolenceEvent evnt)
        {
            //发布两个记录 一个现金记录  一个善心记录
            var tasks = new List<Task>();
            string number = DateTime.Now.ToSerialNumber();
            //现金记录
            tasks.Add(_commandService.SendAsync(new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.AggregateRootId,
                    number,
                    CashTransferType.Incentive,
                    CashTransferStatus.Placed,
                    evnt.IncentiveValue*(1-ConfigSettings.IncentiveFeePersent),//激励善心收取10%手续费
                    evnt.IncentiveValue*ConfigSettings.IncentiveFeePersent,
                    WalletDirection.In,
                    "福豆激励")));
            //善心记录
            tasks.Add(_commandService.SendAsync(new CreateBenevolenceTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.AggregateRootId,
                    number,
                    BenevolenceTransferType.Incentive,
                    BenevolenceTransferStatus.Placed,
                    evnt.BenevolenceDeduct,
                    0,
                    WalletDirection.Out,
                    "福豆指数：{0}".FormatWith(evnt.BenevolenceIndex))));
            //执行所以的任务  
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }
        /// <summary>
        /// 提现申请拒绝
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(WithdrawApplyRejectedEvent evnt)
        {
            return _commandService.SendAsync(
                new CreateCashTransferCommand(
                    GuidUtil.NewSequentialId(),
                    evnt.AggregateRootId,
                    DateTime.Now.ToSerialNumber(),
                    CashTransferType.Refund,
                    CashTransferStatus.Placed,
                    evnt.Amount,
                    0,
                    WalletDirection.In,
                    "提现申请拒绝退款"));
        }
    }
}
