using ECommon.Components;
using ECommon.IO;
using ENode.Commanding;
using ENode.Infrastructure;
using Shop.Commands.Goodses;
using Shop.Domain.Events.Comments;
using System.Threading.Tasks;

namespace Shop.ProcessManagers
{
    [Component]
    public class CommentProcessManager :
        IMessageHandler<CommentCreatedEvent>
    {
        private ICommandService _commandService;

        public CommentProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }
      
        public Task<AsyncTaskResult> HandleAsync(CommentCreatedEvent evnt)
        {
            return _commandService.SendAsync(
                new AcceptNewCommentCommand(evnt.Info.GoodsId,evnt.AggregateRootId));
        }
        
    }
}
