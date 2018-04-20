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
    /// <summary>
    /// 预订单任务
    /// </summary>
    public class WithdrawClearWeekAmountJob : IJob
    {
        private ICommandService _commandService;//C端
        private IWalletQueryService _walletQueryService;//Q 端

        public WithdrawClearWeekAmountJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _walletQueryService = container.Resolve<IWalletQueryService>();
        }
        
        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public  void Execute(IJobExecutionContext context)
        {
            Process();
        }


        private void Process()
        {
            var wallets = _walletQueryService.AllWallets().Where(x=>x.WeekWithdrawAmount>0);
            if (wallets.Any())
            {
                foreach (var wallet in wallets)
                {
                        var command = new ClearWeekWithdrawAmountCommand()
                        {
                            AggregateRootId = wallet.Id
                        };
                        _commandService.SendAsync(command);
                }
            }
        }
    }
}