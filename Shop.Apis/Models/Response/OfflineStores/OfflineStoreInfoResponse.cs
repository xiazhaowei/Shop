using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.OfflineStores
{
    public class OfflineStoreInfoResponse:BaseApiResponse
    {
        public OfflineStore OfflineStore { get; set; }
    }
    public class OfflineStore
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Thumb { get; set; }
        public string Description { get; set; }
        public IList<string> Labels { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }

        public decimal Persent { get; set; }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public decimal TodaySale { get; set; }
        public decimal TotalSale { get; set; }

        public bool IsLocked { get; set; }
        public string CreatedOn { get; set; }
    }
}