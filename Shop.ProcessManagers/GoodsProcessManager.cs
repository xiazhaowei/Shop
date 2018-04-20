using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Goodses;
using Shop.Domain.Events.Goodses;
using System.Threading.Tasks;
using System;
using Shop.Domain.Events.Goodses.Specifications;
using Shop.Common.Enums;

namespace Shop.ProcessManagers
{
    [Component]
    public class GoodsProcessManager 
        //IMessageHandler<GoodsStoreUpdatedEvent>,
        //IMessageHandler<SpecificationsAddedEvent>
    {
        private ICommandService _commandService;

        public GoodsProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }


        /// <summary>
        /// 商家编辑商品 立即下架商品
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        //public Task<AsyncTaskResult> HandleAsync(GoodsStoreUpdatedEvent evnt)
        //{
        //    return _commandService.SendAsync(
        //        new UpdateStatusCommand(evnt.AggregateRootId,GoodsStatus.UnVerify));
        //}
        /// <summary>
        /// 编辑规格下架商品
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        //public Task<AsyncTaskResult> HandleAsync(SpecificationsAddedEvent evnt)
        //{
        //    return _commandService.SendAsync(
        //        new UpdateStatusCommand(evnt.AggregateRootId, GoodsStatus.UnVerify));
        //}
    }
}
