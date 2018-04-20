using System;

namespace Shop.Domain.Models.Goodses.Specifications
{
    /// <summary>
    /// 商品规格
    /// </summary>
    [Serializable]
    public class Specification
    {
        public Guid Id { get; private set; }
        public SpecificationInfo Info { get; set; }
        public int Stock { get; set; }

        public Specification(Guid id, SpecificationInfo info,int stock)
        {
            Id = id;
            Info = info;
            Stock = stock;
        }
    }
}
