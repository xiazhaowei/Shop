using System;

namespace Shop.Api.Models.Request.Announcements
{
    public class EditAnnouncementRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}