using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Apis.ViewModels
{
    public class StoreViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AccessCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
    }
}