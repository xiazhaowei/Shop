using Shop.Common;
using System;

namespace Shop.Domain.Events.Users.ExpressAddresses
{
    /// <summary>
    /// 更新快递地址
    /// </summary>
    [Serializable]
    public  class ExpressAddressUpdatedEvent : ExpressAddressEvent
    {
        public ExpressAddressUpdatedEvent() { }
        public ExpressAddressUpdatedEvent(Guid expressAddressId,ExpressAddressInfo info):base(expressAddressId,info)
        {
        }
    }
}
