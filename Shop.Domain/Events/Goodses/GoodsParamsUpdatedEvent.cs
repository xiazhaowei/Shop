using ENode.Eventing;
using Shop.Domain.Models.Goodses;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsParamsUpdatedEvent:DomainEvent<Guid>
    {
        public IList<GoodsParam> GoodsParams { get; private set; }

        public GoodsParamsUpdatedEvent() { }
        public GoodsParamsUpdatedEvent(IList<GoodsParam> goodsParams)
        {
            GoodsParams = goodsParams;
        }
    }
}
