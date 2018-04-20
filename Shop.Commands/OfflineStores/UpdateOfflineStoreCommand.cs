using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.OfflineStores
{
    public class UpdateOfflineStoreCommand:Command<Guid>
    {
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public IList<string> Labels { get; set; }
        public decimal Persent { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public bool IsLocked { get; set; }

        public UpdateOfflineStoreCommand() { }
        public UpdateOfflineStoreCommand(
            string name,
            string thumb,
            string phone,
            string description,
            IList<string> labels,
            string region,
            string address,
            decimal persent,
            decimal longitude,
            decimal latitude,
            bool isLocked
            )
        {
            Name = name;
            Thumb = thumb;
            Phone = phone;
            Description = description;
            Labels = labels;
            Region = region;
            Address = address;
            Persent = persent;
            Longitude = longitude;
            Latitude = latitude;
            IsLocked = isLocked;
        }
    }
}
