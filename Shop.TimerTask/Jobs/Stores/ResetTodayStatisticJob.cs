using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Stores;
using Shop.QueryServices;
using System.Linq;

namespace Shop.TimerTask.Jobs.Stores
{
    public class ResetTodayStatisticJob: IJob
    {
        private ICommandService _commandService;//C端
        private IStoreQueryService _storeQueryService;//Q 端

        public ResetTodayStatisticJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _storeQueryService = container.Resolve<IStoreQueryService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            ResetTodayStatistic();
        }

        public void ResetTodayStatistic()
        {
            //遍历所有的店铺发送指令
            var stores = _storeQueryService.StoreList().Where(x=>x.TodayOrder>0 || x.TodaySale>0);
            if (stores.Any())
            {
                foreach (var store in stores)
                {
                        var command = new ResetTodayStatisticCommand
                        {
                            AggregateRootId = store.Id
                        };
                        _commandService.SendAsync(command);
                }
            }
        }
    }
}
