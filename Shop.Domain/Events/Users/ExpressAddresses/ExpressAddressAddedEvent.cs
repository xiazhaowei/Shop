using Shop.Common;
using System;

namespace Shop.Domain.Events.Users.ExpressAddresses
{
    /// <summary>
    /// 添加快递地址
    /// </summary>
    [Serializable]
    public  class ExpressAddressAddedEvent : ExpressAddressEvent
    {
        public ExpressAddressAddedEvent() { }
        public ExpressAddressAddedEvent(Guid expressAddressId,ExpressAddressInfo info):base(expressAddressId,info)
        {
        }
    }
}
