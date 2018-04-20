using System;

namespace Shop.Api.Models.Request.Store
{
    public class EditSubjectRequest
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public string SubjectNumber { get; set; }
        public string SubjectPic { get; set; }
    }
}