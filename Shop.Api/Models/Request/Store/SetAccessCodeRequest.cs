using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Store
{
    public class SetAccessCodeRequest
    {
        public Guid Id { get; set; }
        public string AccessCode { get; set; }
    }
}