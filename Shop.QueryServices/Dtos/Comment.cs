using System;

namespace Shop.QueryServices.Dtos
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Single Rate { get; set; }
        public string NickName { get; set; }
        public string Body { get; set; }
        public string Thumbs { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
