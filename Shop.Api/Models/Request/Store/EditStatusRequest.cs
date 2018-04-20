using Shop.Common.Enums;
using System;

namespace Shop.Api.Models.Request.Store
{
    public class EditStatusRequest
    {
        public Guid Id { get; set; }
        public StoreStatus Status { get; set; }
    }
}