using ENode.Commanding;
using Shop.Commands.Grantees;
using Shop.Domain.Models.Grantees;
using System;
using Xia.Common;

namespace Shop.CommandHandlers
{
    public class GranteeCommandHandler :
        ICommandHandler<CreateGranteeCommand>,
        ICommandHandler<AcceptMoneyHelpCommand>,
        ICommandHandler<AddVerificationCommand>,
        ICommandHandler<AddTestifyCommand>,
        ICommandHandler<VerifyCommand>,
        ICommandHandler<UnVerifyCommand>
    {
        public void Handle(ICommandContext context, CreateGranteeCommand command)
        {
            var grantee = new Grantee(GuidUtil.NewSequentialId(), command.Publisher, new GranteeInfo(
                command.Section,
                command.Title,
                command.Description,
                command.Max,
                command.Days,
                DateTime.Now.AddDays(command.Days),
                command.Pics
                ));
            context.Add(grantee);
        }

        public void Handle(ICommandContext context, AcceptMoneyHelpCommand command)
        {
            context.Get<Grantee>(command.AggregateRootId).AcceptMoneyHelp(new MoneyHelp(
                command.UserId,
                command.Amount,
                command.Says
                ));
        }

        public void Handle(ICommandContext context, AddVerificationCommand command)
        {
            context.Get<Grantee>(command.AggregateRootId).AddVerification(new Verification {
                Title=command.Title,
                Pics=command.Pics,
                VerifiedNames=command.VerifiedNames
            });
        }

        public void Handle(ICommandContext context, AddTestifyCommand command)
        {
            context.Get<Grantee>(command.AggregateRootId).AddTestify(new Testify(
                command.UserId,
                command.Relationship,
                command.Name,
                command.Mobile,
                command.CardNumber,
                command.Remark
                ));
        }

        public void Handle(ICommandContext context, VerifyCommand command)
        {
            context.Get<Grantee>(command.AggregateRootId).Verify();
        }

        public void Handle(ICommandContext context, UnVerifyCommand command)
        {
            context.Get<Grantee>(command.AggregateRootId).UnVerify();
        }
    }
}
