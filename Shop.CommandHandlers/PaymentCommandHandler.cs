using ENode.Commanding;
using Shop.Commands.Payments;
using Shop.Domain.Models.Payments;
using System.Linq;

namespace Shop.CommandHandlers
{
    public class PaymentCommandHandler :
        ICommandHandler<CreatePaymentCommand>,
        ICommandHandler<CompletePaymentCommand>,
        ICommandHandler<CancelPaymentCommand>
    {
        public void Handle(ICommandContext context, CreatePaymentCommand command)
        {
            var payment = new Payment(
                command.AggregateRootId,
                command.OrderId,
                command.Description,
                command.TotalAmount,
                command.Lines.Select(x => new PaymentItem(x.Description, x.Amount)).ToList());

            context.Add(payment);
        }
        public void Handle(ICommandContext context, CompletePaymentCommand command)
        {
            context.Get<Payment>(command.AggregateRootId).Complete(command.PayInfo);
        }
        public void Handle(ICommandContext context, CancelPaymentCommand command)
        {
            context.Get<Payment>(command.AggregateRootId).Cancel();
        }
    }
}
