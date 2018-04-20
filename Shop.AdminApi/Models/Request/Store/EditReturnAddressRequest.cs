using System;

namespace Shop.Api.Models.Request.Store
{
    public class EditReturnAddressRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }
}