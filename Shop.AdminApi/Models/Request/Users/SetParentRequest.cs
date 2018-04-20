using System;

namespace Shop.Api.Models.Request.Users
{
    public class SetParentRequest
    {
        public Guid Id { get; set; }
        public string Mobile { get; set; }
    }
}