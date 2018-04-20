using ENode.Commanding;
using Shop.Commands.Comments;
using Shop.Domain.Models.Comments;

namespace Shop.CommandHandlers
{
    public class CommentCommandHandler :
        ICommandHandler<CreateCommentCommand>
    {
        public void Handle(ICommandContext context, CreateCommentCommand command)
        {
            var comment = new Comment(command.AggregateRootId,
                new CommentInfo(
                    command.GoodsId,
                    command.AuthorId,
                    command.Body,
                    command.Thumbs),
                new RateInfo(
                    command.PriceRate,
                    command.DescribeRate,
                    command.QualityRate,
                    command.ExpressRate));

            context.Add(comment);
        }
    }
}
