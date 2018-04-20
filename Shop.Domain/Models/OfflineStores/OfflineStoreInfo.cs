using System.Collections.Generic;

namespace Shop.Domain.Models.OfflineStores
{
    public class OfflineStoreInfo
    {

        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Address { get;  set; }
        public string Description { get;  set; }
        public IList<string> Labels { get; set; }
        public decimal Persent { get;  set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool IsLocked { get; set; }

        public OfflineStoreInfo(string name,
            string thumb,
            string phone,
            string region,
            string address,
            string description,
            IList<string> labels,
            decimal persent,
            decimal longitude,
            decimal latitude,
            bool isLocked)
        {
            Name = name;
            Thumb = thumb;
            Phone = phone;
            Region = region;
            Address = address;
            Description = description;
            Labels = labels;
            Persent = persent;
            Longitude = longitude;
            Latitude = latitude;
            IsLocked = isLocked;
        }
    }
}
