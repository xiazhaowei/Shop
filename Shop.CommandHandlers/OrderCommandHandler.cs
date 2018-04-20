using ENode.Commanding;
using Shop.Commands.Orders;
using Shop.Domain.Models.Orders;
using System.Linq;

namespace Shop.CommandHandlers
{
    public class OrderCommandHandler :
        ICommandHandler<PlaceOrderCommand>,
        ICommandHandler<ConfirmOneReservationCommand>,
        ICommandHandler<ConfirmPaymentCommand>,
        ICommandHandler<MarkAsSuccessCommand>,
        ICommandHandler<MarkAsExpiredCommand>,
        ICommandHandler<CloseOrderCommand>
    {
        private readonly IPricingService _pricingService;

        public OrderCommandHandler(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        public void Handle(ICommandContext context, PlaceOrderCommand command)
        {
            var order = new Order(
                command.AggregateRootId,
                command.UserId,
                command.ExpressAddressInfo,
                command.Specifications.Select(x => new SpecificationQuantity(
                    new Specification(
                    x.SpecificationId,
                    x.GoodsId,
                    x.StoreId,
                    x.GoodsName,
                    x.GoodsPic,
                    x.SpecificationName,
                    x.Price,
                    x.OriginalPrice,
                    x.Benevolence), x.Quantity)),
                _pricingService);

            context.Add(order);
        }
        public void Handle(ICommandContext context, ConfirmOneReservationCommand command)
        {
            context.Get<Order>(command.AggregateRootId).ConfirmOneReservation( command.GoodsId,command.IsReservationSuccess);
        }
    
        public void Handle(ICommandContext context, ConfirmPaymentCommand command)
        {
            context.Get<Order>(command.AggregateRootId).ConfirmPayment(
                command.PayInfo,
                command.IsPaymentSuccess);
        }

        public void Handle(ICommandContext context, MarkAsSuccessCommand command)
        {
            context.Get<Order>(command.AggregateRootId).MarkAsSuccess(command.GoodsId);
        }
        public void Handle(ICommandContext context, CloseOrderCommand command)
        {
            context.Get<Order>(command.AggregateRootId).Close(command.GoodsId);
        }

        public void Handle(ICommandContext context, MarkAsExpiredCommand command)
        {
            context.Get<Order>(command.AggregateRootId).MarkAsExpire();
        }
    }
}
