using System;
using ENode.Commanding;
using System.Collections.Generic;
using Shop.Common.Enums;

namespace Shop.Commands.Goodses
{
    public class UpdateGoodsCommand : Command<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<string> Pics { get; private set; }
        public decimal Price { get; private set; }
        public decimal Benevolence { get; private set; }
        public int SellOut { get; private set; }
        public GoodsStatus Status { get;private set; }
        public string RefusedReason { get;private set; }


        public UpdateGoodsCommand() { }
        public UpdateGoodsCommand(
            string name,
            string description,
            IList<string> pics,
            decimal price,
            decimal benevolence,
            int sellOut,
            GoodsStatus status,
            string refusedReason)
        {
            Name = name;
            Description = description;
            Pics = pics;
            Price = price;
            Benevolence = benevolence;
            SellOut = sellOut;
            Status = status;
            RefusedReason = refusedReason;
        }
    }
}
