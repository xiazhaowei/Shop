using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Response.Admins
{
    public class ListResponse:BaseApiResponse
    {
        public int Total { get; set; }
        public IList<Admin> Admins { get; set; }
    }

    public class Admin
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Portrait { get; set; }
        public string Role { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsLocked { get; set; }
    }
}