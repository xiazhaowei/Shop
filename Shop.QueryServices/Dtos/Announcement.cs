using System;

namespace Shop.QueryServices.Dtos
{
    public class Announcement
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Body { get; set; }
    }
}
