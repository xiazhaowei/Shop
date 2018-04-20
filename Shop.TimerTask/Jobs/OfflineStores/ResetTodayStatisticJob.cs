using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.OfflineStores;
using Shop.QueryServices;
using System.Linq;

namespace Shop.TimerTask.Jobs.OfflineStores
{
    public class ResetTodayStatisticJob: IJob
    {
        private ICommandService _commandService;//C端
        private IOfflineStoreQueryService _offlineStoreQueryService;//Q 端

        public ResetTodayStatisticJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _offlineStoreQueryService = container.Resolve<IOfflineStoreQueryService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            ResetTodayStatistic();
        }

        public void ResetTodayStatistic()
        {
            //遍历所有的店铺发送指令
            var offlintStores = _offlineStoreQueryService.StoreList().Where(x=>x.TodaySale>0);
            if (offlintStores.Any())
            {
                foreach (var offlintStore in offlintStores)
                {
                    var command = new ResetTodayStatisticCommand
                    {
                        AggregateRootId = offlintStore.Id
                    };
                    _commandService.SendAsync(command);
                }
            }
        }
    }
}
