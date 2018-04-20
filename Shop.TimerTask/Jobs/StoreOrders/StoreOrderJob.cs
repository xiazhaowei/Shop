using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Stores.StoreOrders;
using Shop.Common;
using Shop.QueryServices;
using System;
using System.Linq;

namespace Shop.TimerTask.Jobs.StoreOrders
{
    /// <summary>
    /// 预订单任务
    /// </summary>
    public class StoreOrderJob : IJob
    {
        private ICommandService _commandService;//C端
        private IStoreOrderQueryService _storeOrderQueryService;//Q 端

        public StoreOrderJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _storeOrderQueryService = container.Resolve<IStoreOrderQueryService>();
        }
        
        /// <summary>
        /// 计划任务
        /// </summary>
        /// <param name="context"></param>
        public  void Execute(IJobExecutionContext context)
        {
            ProcessConfirmDeliverOrder();
        }

        /// <summary>
        /// 自动确认收货
        /// </summary>
        private void ProcessConfirmDeliverOrder()
        {
            //获取可自动确认收货的订单
            var unConfirmDeliverOrders = _storeOrderQueryService.StoreOrderList().Where(
                x=>x.Status==Common.Enums.StoreOrderStatus.Expressing 
                && x.DeliverTime.HasValue 
                && x.DeliverTime.Value.Add(ConfigSettings.OrderAutoConfirmDeliver)< DateTime.Now);

            if (unConfirmDeliverOrders.Any())
            {
                foreach (var expiredOrder in unConfirmDeliverOrders)
                {
                    var command = new ConfirmDeliverCommand {
                        AggregateRootId=expiredOrder.Id
                    };
                    _commandService.SendAsync(command);
                }
            }
        }
    }
}