using System;
using System.Collections.Generic;

namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 商家编辑商品
    /// </summary>
    public class GoodsStoreEditableInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<string> Pics { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public int Stock { get; private set; }
        public bool IsPayOnDelivery { get; private set; }
        public bool IsInvoice { get; private set; }
        public bool Is7SalesReturn { get; private set; }
        public int Sort { get; private set; }

        public GoodsStoreEditableInfo(
            string name,
            string description,
            IList<string> pics,
            decimal price,
            decimal originalPrice,
            int stock, 
            bool isPayOnDelivery,
            bool isInvoice,
            bool is7SalesReturn,int sort)
        {
            Name = name;
            Description = description;
            Pics = pics;
            Price = price;
            OriginalPrice = originalPrice;
            Stock = stock;
            IsPayOnDelivery = isPayOnDelivery;
            IsInvoice = isInvoice;
            Is7SalesReturn = is7SalesReturn;
            Sort = sort;
        }
    }
}
