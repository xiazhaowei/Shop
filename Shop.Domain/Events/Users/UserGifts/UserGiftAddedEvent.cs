using ENode.Eventing;
using Shop.Common;
using Shop.Domain.Models.Users.UserGifts;
using System;

namespace Shop.Domain.Events.Users.UserGifts
{
    [Serializable]
    public  class UserGiftAddedEvent : DomainEvent<Guid>
    {
        public Guid UserGiftId { get; private set; }
        public GiftInfo GiftInfo { get; private set; }
        public ExpressAddressInfo ExpressAddressInfo { get; private set; }
        public string Remark { get; private set; }

        public UserGiftAddedEvent() { }
        public UserGiftAddedEvent(Guid userGiftId, GiftInfo giftInfo, ExpressAddressInfo expressAddressInfo,string remark="未支付")
        {
            UserGiftId = userGiftId;
            GiftInfo = giftInfo;
            ExpressAddressInfo = expressAddressInfo;
            Remark = remark;
        }
    }
}
