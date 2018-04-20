using Shop.Common.Enums;
using System;

namespace Shop.Domain.Models.Stores
{
    [Serializable]
    public class StoreEditableInfo
    {
        public string Name { get;private set; }
        public string Description { get;private set; }
        public string Address { get;private set; }
        public StoreType Type { get; private set; }

        public StoreEditableInfo(string name,string description,string address,StoreType type)
        {
            Name = name;
            Description = description;
            Address = address;
            Type = type;
        }
    }
}
