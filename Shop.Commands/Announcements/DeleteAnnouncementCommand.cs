using ENode.Commanding;
using System;

namespace Shop.Commands.Announcements
{
    public class DeleteAnnouncementCommand : Command<Guid>
    {
        public DeleteAnnouncementCommand() { }
        public DeleteAnnouncementCommand(Guid id):base(id)
        {
        }
    }
}
