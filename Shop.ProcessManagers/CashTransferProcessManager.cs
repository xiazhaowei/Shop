using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Wallets;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Domain.Events.Wallets;
using Shop.Domain.Events.Wallets.CashTransfers;
using System.Threading.Tasks;

namespace Shop.ProcessManagers
{
    [Component]
    public class CashTransferProcessManager :
        IMessageHandler<CashTransferCreatedEvent>,
        IMessageHandler<NewCashTransferAcceptedEvent>  //钱包接受了新的现金记录  更新现金记录状态为成功
    {
        private ICommandService _commandService;

        public CashTransferProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(CashTransferCreatedEvent evnt)
        {
            //只有提交状态才交给钱包继续处理
            if(evnt.Status==Common.Enums.CashTransferStatus.Placed)
            {
                return _commandService.SendAsync(new AcceptNewCashTransferCommand(evnt.WalletId, evnt.AggregateRootId));
            }
           return Task.FromResult(AsyncTaskResult.Success);
        }
        /// <summary>
        /// 钱包接受记录值，更新记录状态为成功
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public Task<AsyncTaskResult> HandleAsync(NewCashTransferAcceptedEvent evnt)
        {
            return _commandService.SendAsync(
                new SetCashTransferSuccessCommand(evnt.TransferId,evnt.FinallyValue)
                );
        }
    }
}
