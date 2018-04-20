using ENode.Commanding;
using Shop.Commands.Wallets.BenevolenceTransfers;
using Shop.Domain.Models.Wallets.BenevolenceTransfers;

namespace Shop.CommandHandlers
{
    public class BenevolenceTransferCommandHandler :
        ICommandHandler<CreateBenevolenceTransferCommand>,
        ICommandHandler<SetBenevolenceTransferSuccessCommand>
    {
        public void Handle(ICommandContext context, CreateBenevolenceTransferCommand command)
        {
            var cashTransfer = new BenevolenceTransfer(
                command.AggregateRootId,
                command.WalletId,
                command.Number,
                new BenevolenceTransferInfo (
                    command.Amount,
                    command.Fee,
                    command.Direction,
                    command.Remark
                ),
                command.Type,
                command.Status);
            context.Add(cashTransfer);
        }

        public void Handle(ICommandContext context, SetBenevolenceTransferSuccessCommand command)
        {
            context.Get<BenevolenceTransfer>(command.AggregateRootId).SetStateSuccess(command.FinallyValue);
        }
    }
}
