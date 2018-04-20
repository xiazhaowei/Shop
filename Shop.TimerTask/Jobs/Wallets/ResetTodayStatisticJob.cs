using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Wallets;
using Shop.QueryServices;
using System.Linq;

namespace Shop.TimerTask.Jobs.Wallets
{
    public class ResetTodayStatisticJob: IJob
    {
        private ICommandService _commandService;//C端
        private IWalletQueryService _walletQueryService;//Q 端

        public ResetTodayStatisticJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _walletQueryService = container.Resolve<IWalletQueryService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            ResetTodayStatistic();
        }

        public void ResetTodayStatistic()
        {
            //遍历所有的钱包发送指令
            var wallets = _walletQueryService.ListPage().Where(x=>x.YesterdayEarnings>0 || x.TodayBenevolenceAdded>0);
            if (wallets.Any())
            {
                foreach (var wallet in wallets)
                {
                        var command = new ResetTodayStatisticCommand
                        {
                            AggregateRootId = wallet.Id
                        };
                        _commandService.SendAsync(command);
                }
            }
        }
    }
}
