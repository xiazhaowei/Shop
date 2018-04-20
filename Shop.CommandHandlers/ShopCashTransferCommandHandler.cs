using ENode.Commanding;
using Shop.Commands.Wallets.ShopCashTransfers;
using Shop.Domain.Models.Wallets.ShopCashTransfers;

namespace Shop.CommandHandlers
{
    public class ShopCashTransferCommandHandler :
        ICommandHandler<CreateShopCashTransferCommand>,
        ICommandHandler<SetShopCashTransferSuccessCommand>
    {
        public void Handle(ICommandContext context, CreateShopCashTransferCommand command)
        {
            var cashTransfer = new ShopCashTransfer(
                command.AggregateRootId,
                command.WalletId,
                command.Number,
                new ShopCashTransferInfo (command.Amount,
                command.Fee,
                command.Direction,
                command.Remark),
                command.Type,
                command.Status);
            context.Add(cashTransfer);
        }

        public void Handle(ICommandContext context, SetShopCashTransferSuccessCommand command)
        {
            context.Get<ShopCashTransfer>(command.AggregateRootId).SetStateSuccess(command.FinallyValue);
        }
    }
}
