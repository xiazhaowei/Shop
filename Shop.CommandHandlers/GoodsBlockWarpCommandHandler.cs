using ENode.Commanding;
using Shop.Commands.GoodsBlocks;
using Shop.Domain.Models.GoodsBlocks;

namespace Shop.CommandHandlers
{
    public class GoodsBlockWarpCommandHandler :
        ICommandHandler<CreateGoodsBlockWarpCommand>,
        ICommandHandler<UpdateGoodsBlockWarpCommand>,
        ICommandHandler<DeleteGoodsBlockWarpCommand>
    {
        public void Handle(ICommandContext context, CreateGoodsBlockWarpCommand command)
        {
            var goodsBlockWarp = new GoodsBlockWarp(
                command.AggregateRootId,
                new GoodsBlockWarpInfo(
                command.Name,
                command.Style,
                command.GoodsBlocks,
                command.IsShow,
                command.Sort));
            
            //将领域对象添加到上下文中
            context.Add(goodsBlockWarp);
        }

        public void Handle(ICommandContext context, UpdateGoodsBlockWarpCommand command)
        {
            context.Get<GoodsBlockWarp>(command.AggregateRootId).Update(
                new GoodsBlockWarpInfo(
                    command.Name,
                    command.Style,
                    command.GoodsBlocks,
                    command.IsShow,
                    command.Sort));
        }

        public void Handle(ICommandContext context, DeleteGoodsBlockWarpCommand command)
        {
            context.Get<GoodsBlockWarp>(command.AggregateRootId).Delete();
        }
    }
}
