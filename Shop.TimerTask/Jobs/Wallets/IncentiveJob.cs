using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.BenevolenceIndexs;
using Shop.Commands.Wallets;
using Shop.Common;
using Shop.QueryServices;
using System;
using System.Linq;
using Xia.Common;

namespace Shop.TimerTask.Jobs.Wallets
{
    public class IncentiveJob: IJob
    {
        private ICommandService _commandService;//C端
        private IWalletQueryService _walletQueryService;//Q 端
       

        public IncentiveJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _walletQueryService = container.Resolve<IWalletQueryService>();
        }

        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Incentive();
        }

        public void Incentive()
        {
            var benevolenceIndex = RandomArray.BenevolenceIndex();

            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                //周日两倍激励
                benevolenceIndex = benevolenceIndex * 2;
            }

            //善心指数判断
            if (benevolenceIndex <= 0 || benevolenceIndex >= 1)
            {
                throw new Exception("善心指数异常");
            }
            //所有待激励的钱包 福豆>1  钱包未锁定
            var wallets = _walletQueryService.ListPage().Where(x=>x.Benevolence>1&& x.IsFreeze==Common.Enums.Freeze.UnFreeze);
            //遍历所有的钱包发送激励指令
            if (wallets.Any())
            {
                var totalBenevolenceAmount = wallets.Sum(x => x.Benevolence);
                //创建激励记录
                _commandService.SendAsync(new CreateBenevolenceIndexCommand(
                    GuidUtil.NewSequentialId(),
                    benevolenceIndex,
                    totalBenevolenceAmount
                    ));

                foreach (var wallet in wallets)
                {
                    if(wallet.Benevolence>1)
                    {
                        var command = new IncentiveBenevolenceCommand(wallet.Id, benevolenceIndex);
                        _commandService.SendAsync(command);
                    }
                }
            }
        }
    }
}
