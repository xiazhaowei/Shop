using ENode.Commanding;
using System;

namespace Shop.Commands.Announcements
{
    public class CreateAnnouncementCommand : Command<Guid>
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public CreateAnnouncementCommand() { }
        public CreateAnnouncementCommand(Guid id,string title,string body):base(id)
        {
            Title = title;
            Body = body;
        }
    }
}
