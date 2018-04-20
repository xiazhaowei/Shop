using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Store
{
    public class AdminEditRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public StoreType Type { get; set; }
    }
}