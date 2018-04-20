using ECommon.Components;
using ENode.Infrastructure;
using Shop.Domain.Events.ThirdCurrencys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommon.IO;
using ENode.Commanding;
using Xia.Common;
using Shop.Commands.Wallets.ShopCashTransfers;
using Xia.Common.Extensions;
using Shop.Common.Enums;

namespace Shop.ProcessManagers
{
    [Component]
    public class ThirdCurrencyProcessManager :
        IMessageHandler<NewThirdCurrencyImportLogEvent>
    {
        private ICommandService _commandService;

        public ThirdCurrencyProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public Task<AsyncTaskResult> HandleAsync(NewThirdCurrencyImportLogEvent evnt)
        {
            var number = DateTime.Now.ToSerialNumber();

            return _commandService.SendAsync(new CreateShopCashTransferCommand(
                        GuidUtil.NewSequentialId(),
                        evnt.LogInfo.WalletId,
                        number,
                        ShopCashTransferType.Charge,
                        ShopCashTransferStatus.Placed,
                        evnt.LogInfo.ShopCashAmount,
                        0,
                        WalletDirection.In,
                        "导入"));
        }
    }
}
