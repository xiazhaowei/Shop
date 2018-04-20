using System;

namespace Shop.QueryServices.Dtos
{
    /// <summary>
    /// 快递地址 DTO
    /// </summary>
    public class ExpressAddress
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}
