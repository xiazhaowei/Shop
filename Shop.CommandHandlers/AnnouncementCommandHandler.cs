using ENode.Commanding;
using Shop.Commands.Announcements;
using Shop.Domain.Models.Announcements;

namespace Shop.CommandHandlers
{
    public class AnnouncementCommandHandler :
        ICommandHandler<CreateAnnouncementCommand>,
        ICommandHandler<UpdateAnnouncementCommand>,
        ICommandHandler<DeleteAnnouncementCommand>
    {
        public void Handle(ICommandContext context, CreateAnnouncementCommand command)
        {
            var announcement = new Announcement(command.AggregateRootId, command.Title, command.Body);
            context.Add(announcement);
        }

        public void Handle(ICommandContext context, UpdateAnnouncementCommand command)
        {
            context.Get<Announcement>(command.AggregateRootId).Update(command.Title, command.Body);
        }

        public void Handle(ICommandContext context, DeleteAnnouncementCommand command)
        {
            context.Get<Announcement>(command.AggregateRootId).Delete();
        }
    }
}
