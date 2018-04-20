using System;

namespace Shop.Domain.Models.Stores
{
    [Serializable]
    public class StoreInfo
    {
        public string Name { get; private set; }
        /// <summary>
        /// 访问密码
        /// </summary>
        public string AccessCode { get;  set; }
        public string Description { get; private set; }
        public string Region { get; private set; }
        public string Address { get; private set; }

        public StoreInfo(string accessCode,string name,string description,string region,string address)
        {
            AccessCode = accessCode;
            Name = name;
            Description = description;
            Region = region;
            Address = address;
        }
    }
}
