using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Domain.Events.Partners;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Xia.Common.Extensions;
using Xia.Common;
using Shop.Common.Enums;
using Shop.Common;

namespace Shop.ProcessManagers
{
    [Component]
    public class PartnerProcessManager :
        IMessageHandler<AcceptedNewBalanceEvent> //代理分成
    {
        private ICommandService _commandService;

        public PartnerProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
        
        //获得分红
        public Task<AsyncTaskResult> HandleAsync(AcceptedNewBalanceEvent evnt)
        {
            var tasks = new List<Task>();
            string number = DateTime.Now.ToSerialNumber();
            if (evnt.CashBalanceAmount > 0)
            {
                //现金奖励
                tasks.Add(_commandService.SendAsync(new CreateCashTransferCommand(
                        GuidUtil.NewSequentialId(),
                        evnt.WalletId,
                        number,
                        CashTransferType.UnionAward,
                        CashTransferStatus.Placed,
                        evnt.CashBalanceAmount * (1 - ConfigSettings.BalanceFeePersent),//分红收取10%手续费
                        evnt.CashBalanceAmount * ConfigSettings.BalanceFeePersent,
                        WalletDirection.In,
                        "代理分红")));
            }
            if (evnt.BenevolenceBalanceAmount > 0)
            {
                //福豆奖励
                tasks.Add(_commandService.SendAsync(new CreateBenevolenceTransferCommand(
                        GuidUtil.NewSequentialId(),
                        evnt.WalletId,
                        number,
                        BenevolenceTransferType.UnionAward,
                        BenevolenceTransferStatus.Placed,
                        evnt.BenevolenceBalanceAmount * (1 - ConfigSettings.BalanceFeePersent),//分红收取10%手续费
                        evnt.BenevolenceBalanceAmount * ConfigSettings.BalanceFeePersent,
                        WalletDirection.In,
                        "代理分红")));
            }
            
            //执行所以的任务  
            Task.WaitAll(tasks.ToArray());
            return Task.FromResult(AsyncTaskResult.Success);
        }
    }
}
