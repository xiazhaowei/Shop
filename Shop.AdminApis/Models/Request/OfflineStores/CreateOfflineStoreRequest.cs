using System;
using System.Collections.Generic;

namespace Shop.Api.Models.Request.OfflineStores
{
    public class CreateOfflineStoreRequest
    {
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public IList<string> Labels { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public decimal Persent { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool IsLocked { get; set; }
    }
}