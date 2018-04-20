using System;

namespace Shop.QueryServices.Dtos
{
    public class GoodsParam
    {
        public Guid Id { get; set; }
        public Guid GoodsId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
