using ENode.Commanding;
using Shop.Commands.ThirdCurrencys;
using Shop.Domain.Models.ThirdCurrencys;

namespace Shop.CommandHandlers
{
    public class ThirdCurrencyCommandHandler :
        ICommandHandler<CreateThirdCurrencyCommand>,
        ICommandHandler<UpdateThirdCurrencyCommand>,
        ICommandHandler<DeleteThirdCurrencyCommand>,
        ICommandHandler<AcceptNewImportCommand>,
        ICommandHandler<ChargeThirdCurrencyCommand>
    {
        public void Handle(ICommandContext context, CreateThirdCurrencyCommand command)
        {
            var thirdCurrency = new ThirdCurrency(command.AggregateRootId, new ThirdCurrencyInfo(
                command.Name,
                command.Icon,
                command.ComponyName,
                command.Conversion,
                command.IsLocked,
                command.Remark
                ));
            context.Add(thirdCurrency);
        }

        public void Handle(ICommandContext context, UpdateThirdCurrencyCommand command)
        {
            context.Get<ThirdCurrency>(command.AggregateRootId).Update(new ThirdCurrencyInfo(
                command.Name,
                command.Icon,
                command.ComponyName,
                command.Conversion,
                command.IsLocked,
                command.Remark
                ));
        }

        public void Handle(ICommandContext context, DeleteThirdCurrencyCommand command)
        {
            context.Get<ThirdCurrency>(command.AggregateRootId).Delete();
        }

        public void Handle(ICommandContext context, AcceptNewImportCommand command)
        {
            context.Get<ThirdCurrency>(command.AggregateRootId).AcceptNewImport(new ImportInfo(
                command.WalletId,
                command.UserId,
                command.Mobile,
                command.Account,
                command.Amount
                ));
        }

        public void Handle(ICommandContext context, ChargeThirdCurrencyCommand command)
        {
            context.Get<ThirdCurrency>(command.AggregateRootId).Charge(command.Amount);
        }
    }
}
