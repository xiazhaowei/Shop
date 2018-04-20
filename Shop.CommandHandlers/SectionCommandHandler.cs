using ENode.Commanding;
using Shop.Commands.Stores.Sections;
using Shop.Domain.Models.Stores.Sections;

namespace Shop.CommandHandlers
{
    public class SectionCommandHandler :
        ICommandHandler<CreateSectionCommand>,
        ICommandHandler<ChangeSectionCommand>
    {
        public void Handle(ICommandContext context, CreateSectionCommand command)
        {
            context.Add(new Section(command.AggregateRootId, command.Name, command.Description));
        }
        public void Handle(ICommandContext context, ChangeSectionCommand command)
        {
            context.Get<Section>(command.AggregateRootId).ChangeSection(command.Name, command.Description);
        }
    }
}
