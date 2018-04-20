using Shop.Common;
using System;

namespace Shop.Domain.Models.Users.ExpressAddresses
{
    /// <summary>
    /// 快递地址
    /// </summary>
    [Serializable]
    public class ExpressAddress
    {
        public Guid Id { get; private set; }
        public ExpressAddressInfo Info { get;  set; }

        public ExpressAddress(Guid id)
        {
            Id = id;
        }
        public ExpressAddress(Guid id,ExpressAddressInfo info)
        {
            Id = id;
            Info = info;
        }
    }
}
