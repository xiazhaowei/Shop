using ENode.Commanding;
using Shop.Commands.BenevolenceIndexs;
using Shop.Domain.Models.BenevolenceIndexs;

namespace Shop.CommandHandlers
{
    public class BenevolenceIndexCommandHandler:
        ICommandHandler<CreateBenevolenceIndexCommand>
    {
        public void Handle(ICommandContext context, CreateBenevolenceIndexCommand command)
        {
            var benevolenceIndex = new BenevolenceIndex(
                command.AggregateRootId,
                command.BenevolenceIndex,
                command.BenevolenceAmount);
            context.Add(benevolenceIndex);
        }
        
    }
}
