using ENode.Commanding;
using Shop.Commands.Partners;
using Shop.Domain.Models.Partners;

namespace Shop.CommandHandlers
{
    public class PartnerCommandHandler :
        ICommandHandler<CreatePartnerCommand>,
        ICommandHandler<UpdatePartnerCommand>,
        ICommandHandler<DeletePartnerCommand>,
        ICommandHandler<AcceptNewBalanceCommand>
    {
        public void Handle(ICommandContext context, CreatePartnerCommand command)
        {
            var partner = new Partner(
                command.AggregateRootId,
                command.UserId,
                command.WalletId,
                new PartnerInfo(
                    command.Mobile,
                    command.Region,
                    command.Level, 
                    command.Persent, 
                    command.CashPersent,
                    command.BalanceInterval,
                    command.Remark,
                    command.IsLocked)
                );
            context.Add(partner);
        }
        public void Handle(ICommandContext context, AcceptNewBalanceCommand command)
        {
            context.Get<Partner>(command.AggregateRootId).AcceptNewBalance(command.Amount);
        }

        public void Handle(ICommandContext context, UpdatePartnerCommand command)
        {
            context.Get<Partner>(command.AggregateRootId).Update(new PartnerInfo(
                    command.Mobile,
                    command.Region,
                    command.Level,
                    command.Persent,
                    command.CashPersent,
                    command.BalanceInterval,
                    command.Remark,
                    command.IsLocked
                ));
        }

        public void Handle(ICommandContext context, DeletePartnerCommand command)
        {
            context.Get<Partner>(command.AggregateRootId).Delete();
        }
    }
}
