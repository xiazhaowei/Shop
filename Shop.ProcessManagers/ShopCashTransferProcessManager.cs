using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Domain.Events.Wallets;
using Shop.Domain.Events.Wallets.ShopCashTransfers;
using System.Threading.Tasks;

namespace Shop.ProcessManagers
{
    [Component]
    public class ShopCashTransferProcessManager :
        IMessageHandler<ShopCashTransferCreatedEvent>,
        IMessageHandler<NewShopCashTransferAcceptedEvent>  //钱包接受了新的记录  更新记录状态为成功
    {
        private ICommandService _commandService;

        public ShopCashTransferProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(ShopCashTransferCreatedEvent evnt)
        {
            //只有提交状态才交给钱包继续处理
            if(evnt.Status==Common.Enums.ShopCashTransferStatus.Placed)
            {
                return _commandService.SendAsync(new AcceptNewShopCashTransferCommand(evnt.WalletId, evnt.AggregateRootId));
            }
           return Task.FromResult(AsyncTaskResult.Success);
        }
        /// <summary>
        /// 钱包接受记录值，更新记录状态为成功
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(NewShopCashTransferAcceptedEvent evnt)
        {
            return _commandService.SendAsync(
                new SetShopCashTransferSuccessCommand(evnt.TransferId,evnt.FinallyValue)
                );
        }
    }
}
