using ENode.Commanding;
using Shop.Commands.Wallets.CashTransfers;
using Shop.Domain.Models.Wallets.CashTransfers;

namespace Shop.CommandHandlers
{
    public class CashTransferCommandHandler :
        ICommandHandler<CreateCashTransferCommand>,
        ICommandHandler<SetCashTransferSuccessCommand>
    {
        public void Handle(ICommandContext context, CreateCashTransferCommand command)
        {
            var cashTransfer = new CashTransfer(
                command.AggregateRootId,
                command.WalletId,
                command.Number,
                new CashTransferInfo (command.Amount,
                command.Fee,
                command.Direction,
                command.Remark),
                command.Type,
                command.Status);
            context.Add(cashTransfer);
        }

        public void Handle(ICommandContext context, SetCashTransferSuccessCommand command)
        {
            context.Get<CashTransfer>(command.AggregateRootId).SetStateSuccess(command.FinallyValue);
        }
    }
}
