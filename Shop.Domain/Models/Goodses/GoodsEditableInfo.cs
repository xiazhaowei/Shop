using Shop.Common.Enums;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Models.Goodses
{
    /// <summary>
    /// 平台管理商品
    /// </summary>
    public class GoodsEditableInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<string> Pics { get; private set; }
        public decimal Price { get; private set; }
        public decimal Benevolence { get; private set; }
        public int SellOut { get; private set; }
        public GoodsStatus Status { get; private set; }
        public string RefusedReason { get; private set; }

        public GoodsEditableInfo(
            string name,
            string description,
            IList<string> pics,
            decimal price,
            decimal benevolence,
            int sellOut,
            GoodsStatus status,
            string refusedReason
            )
        {
            Name = name;
            Description = description;
            Pics = pics;
            Price =price;
            Benevolence = benevolence;
            SellOut = sellOut;
            Status = status;
            RefusedReason = refusedReason;
        }
    }
}
