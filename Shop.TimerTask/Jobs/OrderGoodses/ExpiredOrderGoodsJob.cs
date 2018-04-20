using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Stores.StoreOrders.OrderGoodses;
using Shop.QueryServices;
using System.Linq;

namespace Shop.TimerTask.Jobs.OrderGoodses
{
    /// <summary>
    /// 预订单任务
    /// </summary>
    public class ExpiredOrderGoodsJob : IJob
    {
        private ICommandService _commandService;//C端
        private IOrderGoodsQueryService _orderGoodsQueryService;//Q 端

        public ExpiredOrderGoodsJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _orderGoodsQueryService = container.Resolve<IOrderGoodsQueryService>();
        }
        
        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public  void Execute(IJobExecutionContext context)
        {
            ProcessExpiredOrderGoodses();
        }


        private void ProcessExpiredOrderGoodses()
        {
            var expiredNormalGoodses = _orderGoodsQueryService.ExpiredNormalGoodses();
            if (expiredNormalGoodses.Any())
            {
                foreach (var expiredOrderGoods in expiredNormalGoodses)
                {
                    var command = new MarkAsExpireCommand() {
                        AggregateRootId=expiredOrderGoods.Id
                    };
                    _commandService.SendAsync(command);
                }
            }
        }
    }
}