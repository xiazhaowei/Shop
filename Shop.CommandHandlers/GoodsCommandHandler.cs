using ENode.Commanding;
using Shop.Commands.Goodses;
using Shop.Commands.Goodses.Specifications;
using Shop.Common;
using Shop.Domain.Models.Comments;
using Shop.Domain.Models.Goodses;
using System.Linq;

namespace Shop.CommandHandlers
{
    public class GoodsCommandHandler :
        ICommandHandler<CreateGoodsCommand>,
        ICommandHandler<StoreUpdateGoodsCommand>,
        ICommandHandler<UpdateGoodsCommand>,
        ICommandHandler<UpdateStatusCommand>,
        ICommandHandler<DeleteGoodsCommand>,

        ICommandHandler<PublishGoodsCommand>,
        ICommandHandler<UnpublishGoodsCommand>,

        ICommandHandler<AddSpecificationCommand>,
        ICommandHandler<AddSpecificationsCommand>,
        ICommandHandler<UpdateSpecificationCommand>,
        ICommandHandler<UpdateSpecificationsCommand>,
        ICommandHandler<UpdateParamsCommand>,

        ICommandHandler<MakeSpecificationReservationCommand>,
        ICommandHandler<CommitSpecificationReservationCommand>,
        ICommandHandler<CancelSpecificationReservationCommand>,

        ICommandHandler<AcceptNewCommentCommand>
    {
        public void Handle(ICommandContext context, CreateGoodsCommand command)
        {
            //创建聚合跟对象
            var goods = new Goods(command.AggregateRootId,command.StoreId, 
                command.CategoryIds,
                new GoodsInfo(
                command.Name,
                command.Description,
                command.Pics,
                command.Price,//建议售价,
                command.OriginalPrice,
                command.Price / ConfigSettings.BenevolenceValue,//善心
                command.Stock,
                0,//已销售量
                command.IsPayOnDelivery,
                command.IsInvoice,
                command.Is7SalesReturn,
                command.Sort));
            //添加到上下文
            context.Add(goods);
        }
        public void Handle(ICommandContext context, StoreUpdateGoodsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).Update(
                command.CategoryIds,
                new GoodsStoreEditableInfo(
                command.Name,
                command.Description,
                command.Pics,
                command.Price,
                command.OriginalPrice,
                command.Stock,
                command.IsPayOnDelivery,
                command.IsInvoice,
                command.Is7SalesReturn,
                command.Sort));
        }
        public void Handle(ICommandContext context, UpdateGoodsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).AdminUpdate(
                new GoodsEditableInfo(
                command.Name,
                command.Description,
                command.Pics,
                command.Price,
                command.Benevolence,
                command.SellOut,
                command.Status,
                command.RefusedReason));
        }
        public void Handle(ICommandContext context, PublishGoodsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).Publish();
        }
        public void Handle(ICommandContext context, UnpublishGoodsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).Unpublish();
        }
        public void Handle(ICommandContext context, AddSpecificationCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).AddSpecification(
                new Domain.Models.Goodses.Specifications.SpecificationInfo(
                    command.Name,
                    command.Value,
                    command.Thumb,
                    command.Price,
                    command.OriginalPrice,
                    command.Benevolence,
                    command.Number,
                    command.BarCode
                ),
                command.Stock);
        }
       
        public void Handle(ICommandContext context, UpdateSpecificationCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).UpdateSpecification(
                command.SpecificationId,
                new Domain.Models.Goodses.Specifications.SpecificationInfo(
                    command.Name, 
                    command.Value,
                    command.Thumb,
                    command.Price,
                    command.OriginalPrice,
                    command.Benevolence,
                    command.Number,
                    command.BarCode),command.Stock);
        }
        /// <summary>
        /// 处理商品预定指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        public void Handle(ICommandContext context, MakeSpecificationReservationCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).MakeReservation(
                command.ReservationId, 
                command.Specifications.Select(x => new ReservationItem(x.SpecificationId, x.Quantity)).ToList());
        }
        public void Handle(ICommandContext context, CommitSpecificationReservationCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).CommitReservation(command.ReservationId);
        }
        public void Handle(ICommandContext context, CancelSpecificationReservationCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).CancelReservation(command.ReservationId);
        }
        public void Handle(ICommandContext context, AcceptNewCommentCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).AcceptNewComment(context.Get<Comment>(command.CommentId));
        }

        public void Handle(ICommandContext context, AddSpecificationsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).AddSpecifications(command.Specifications.Select(x =>new Domain.Models.Goodses.Specifications.Specification(
                x.Id, 
                new Domain.Models.Goodses.Specifications.SpecificationInfo(
                    x.Name,
                    x.Value,
                    x.Thumb,
                    x.Price,
                    x.OriginalPrice,
                    x.Benevolence,
                    x.Number,
                    x.BarCode
                ),
                x.Stock)).ToList());
        }

        public void Handle(ICommandContext context, UpdateParamsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).UpdateParams(command.GoodsParams.Select(x => new GoodsParam(
                x.Name,
                x.Value)).ToList());
        }

        public void Handle(ICommandContext context, UpdateSpecificationsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).UpdateSpecifications(
                command.Specifications.Select(x => new Domain.Models.Goodses.Specifications.Specification(
                x.Id,
                new Domain.Models.Goodses.Specifications.SpecificationInfo(
                   x.Name,
                   x.Value,
                   x.Thumb,
                   x.Price,
                   x.OriginalPrice,
                   x.Benevolence,
                   x.Number,
                   x.BarCode),
                x.Stock
                )).ToList(),
                command.IsAdmin);
        }

        public void Handle(ICommandContext context, UpdateStatusCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).UpdateStatus(command.Status);
        }

        public void Handle(ICommandContext context, DeleteGoodsCommand command)
        {
            context.Get<Goods>(command.AggregateRootId).Delete();
        }
    }
}
