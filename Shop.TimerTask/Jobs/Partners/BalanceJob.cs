using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Commanding;
using Quartz;
using Shop.Commands.Partners;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Linq;

namespace Shop.TimerTask.Jobs.Partners
{
    public class BalanceJob : IJob
    {
        private ICommandService _commandService;//C端
        private IStoreOrderQueryService _storeOrderQueryService;//Q 端
        private IPartnerQueryService _partnerQueryService;//Q 端

        public BalanceJob()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            _commandService = container.Resolve<ICommandService>();
            _storeOrderQueryService = container.Resolve<IStoreOrderQueryService>();
            _partnerQueryService = container.Resolve<IPartnerQueryService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            Balance();
        }

        //分红任务
        public void Balance()
        {
            //获取所有的成功订单
            var storeOrders = _storeOrderQueryService.StoreOrders().Where(x=>x.Status==StoreOrderStatus.Success);
            if (storeOrders.Any())
            {
                //获取所有的可用代理人
                var partners = _partnerQueryService.Partners().Where(x => !x.IsLocked);
                foreach (var partner in partners)
                {
                    var partnerBalanceDate= partner.BalancedDate.AddDays(partner.BalanceInterval);
                    //判断是否到达分红期
                    if (DateTime.Now>partnerBalanceDate || DateTime.Now.Date.Equals(partnerBalanceDate.Date))
                    {
                        //分红期内销售额
                        var amount = storeOrders.Where(
                            x => x.ExpressRegion.Contains(partner.Region) 
                            && x.CreatedOn > partner.BalancedDate 
                            && x.CreatedOn < DateTime.Now).Sum(x => x.Total);
                        var command = new AcceptNewBalanceCommand(amount)
                        {
                            AggregateRootId = partner.Id
                        };
                        _commandService.SendAsync(command);
                    }
                }
            }
        }
    }
}
