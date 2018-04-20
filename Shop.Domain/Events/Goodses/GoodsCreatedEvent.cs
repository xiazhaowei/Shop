using Shop.Domain.Models.Goodses;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Events.Goodses
{
    [Serializable]
    public class GoodsCreatedEvent:GoodsEvent
    {
        public Guid StoreId { get; private set; }
        public IList<Guid> CategoryIds { get; private set; }

        public GoodsCreatedEvent() { }
        public GoodsCreatedEvent(Guid storeId,IList<Guid> categoryIds,GoodsInfo info):base(info)
        {
            StoreId = storeId;
            CategoryIds = categoryIds;
        }
    }
}
