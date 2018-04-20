using ENode.Commanding;
using Shop.Commands.Carts;
using Shop.Domain.Models.Carts;

namespace Shop.CommandHandlers
{
    public class CartCommandHandler :
        ICommandHandler<CreateCartCommand>,
        ICommandHandler<AddCartGoodsCommand>,
        ICommandHandler<RemoveCartGoodsCommand>
    {
        public void Handle(ICommandContext context, CreateCartCommand command)
        {
            var cart = new Cart(command.AggregateRootId, command.UserId);
            context.Add(cart);
        }

        public void Handle(ICommandContext context, AddCartGoodsCommand command)
        {
            context.Get<Cart>(command.AggregateRootId).AddGoods(
                new Domain.Models.Carts.CartGoodses.CartGoodsInfo(
                    command.StoreId,
                    command.GoodsId,
                    command.SpecificationId,
                    command.GoodsName,
                    command.GoodsPic,
                    command.SpecificationName,
                    command.Price,
                    command.OriginalPrice,
                    command.Quantity,
                    command.Stock,
                    command.Benevolence));
        }

        public void Handle(ICommandContext context, RemoveCartGoodsCommand command)
        {
            context.Get<Cart>(command.AggregateRootId).RemoveGoods(command.CartGoodsId);
        }
    }
}
