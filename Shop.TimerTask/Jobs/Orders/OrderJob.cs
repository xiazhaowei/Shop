using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Orders;
using Shop.QueryServices;
using System.Linq;

namespace Shop.TimerTask.Jobs.Orders
{
    /// <summary>
    /// 预订单任务
    /// </summary>
    public class OrderJob : IJob
    {
        private ICommandService _commandService;//C端
        private IOrderQueryService _orderQueryService;//Q 端

        public OrderJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _orderQueryService = container.Resolve<IOrderQueryService>();
        }
        
        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public  void Execute(IJobExecutionContext context)
        {
            ProcessExpiredOrder();
        }


        private void ProcessExpiredOrder()
        {
            //获取所有过期未支付的预订单
            var expiredUnPayOrders = _orderQueryService.ExpiredUnPayOrders();
            if (expiredUnPayOrders.Any())
            {
                foreach (var expiredOrder in expiredUnPayOrders)
                {
                    var command = new MarkAsExpiredCommand(expiredOrder.OrderId);
                    _commandService.SendAsync(command);
                }
            }
        }
    }
}