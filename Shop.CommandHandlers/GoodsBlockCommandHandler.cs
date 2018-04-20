using ENode.Commanding;
using Shop.Commands.GoodsBlocks;
using Shop.Domain.Models.GoodsBlocks;

namespace Shop.CommandHandlers
{
    public class GoodsBlockCommandHandler :
        ICommandHandler<CreateGoodsBlockCommand>,
        ICommandHandler<UpdateGoodsBlockCommand>,
        ICommandHandler<DeleteGoodsBlockCommand>
    {
        public void Handle(ICommandContext context, CreateGoodsBlockCommand command)
        {
            var goodsBlock = new GoodsBlock(
                command.AggregateRootId,
                new GoodsBlockInfo(command.Name,
                command.Thumb,
                command.Banner,
                command.Layout,
                command.Goodses,
                command.IsShow,
                command.Sort));
            
            //将领域对象添加到上下文中
            context.Add(goodsBlock);
        }

        public void Handle(ICommandContext context, UpdateGoodsBlockCommand command)
        {
            context.Get<GoodsBlock>(command.AggregateRootId).Update(
                new GoodsBlockInfo(
                    command.Name,
                    command.Thumb,
                    command.Banner,
                    command.Layout,
                    command.Goodses,
                    command.IsShow,
                    command.Sort));
        }

        public void Handle(ICommandContext context, DeleteGoodsBlockCommand command)
        {
            context.Get<GoodsBlock>(command.AggregateRootId).Delete();
        }
    }
}
