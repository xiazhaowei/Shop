using Shop.Common;
using System;

namespace Shop.Domain.Models.Users.UserGifts
{
    public class UserGift
    {
        public Guid Id { get;private set; }
        public GiftInfo GiftInfo { get; private set; }
        public ExpressAddressInfo ExpressAddressInfo { get; private set; }

        /// <summary>
        /// 未发货  已发货 调换
        /// </summary>
        public string Remark { get;  set; }

        public UserGift(Guid id,GiftInfo giftInfo,ExpressAddressInfo expressAddressInfo,string remark)
        {
            Id = id;
            GiftInfo = giftInfo;
            ExpressAddressInfo = expressAddressInfo;
            Remark = remark;
        }
    }
}
