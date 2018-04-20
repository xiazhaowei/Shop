using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Announcements
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Announcement> Announcements { get; set; }
    }

    public class Announcement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}