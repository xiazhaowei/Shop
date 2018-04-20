using System;

namespace Shop.Domain.Models.Stores
{
    [Serializable]
    public class StoreCustomerEditableInfo
    {
        public string Name { get;private set; }
        public string Description { get;private set; }
        public string Address { get;private set; }

        public StoreCustomerEditableInfo(string name,string description,string address)
        {
            Name = name;
            Description = description;
            Address = address;
        }
    }
}
