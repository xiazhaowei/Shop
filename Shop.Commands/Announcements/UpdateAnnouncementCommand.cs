using ENode.Commanding;
using System;

namespace Shop.Commands.Announcements
{
    public class UpdateAnnouncementCommand : Command<Guid>
    {
        public string Title { get; private set; }
        public string Body { get; private set; }


        public UpdateAnnouncementCommand() { }
        public UpdateAnnouncementCommand(string title,string body)
        {
            Title = title;
            Body = body;
        }
    }
}
