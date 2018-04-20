using System;
using System.Collections.Generic;
using ENode.Infrastructure;

namespace Shop.Domain.Models.PublishableExceptions
{
    /// <summary>
    /// 商品规格不足 异常
    /// </summary>
    [Serializable]
    public class SpecificationInsufficientException : PublishableException
    {
        public Guid GoodsId { get; private set; }
        public Guid ReservationId { get; private set; }

        /// <summary>
        /// 商品规格可用数量不足 异常
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="reservationId"></param>
        public SpecificationInsufficientException(Guid goodsId, Guid reservationId) : base()
        {
            GoodsId = goodsId;
            ReservationId = reservationId;
        }

        public override void RestoreFrom(IDictionary<string, string> serializableInfo)
        {
            GoodsId = Guid.Parse(serializableInfo["GoodsId"]);
            ReservationId = Guid.Parse(serializableInfo["ReservationId"]);
        }
        public override void SerializeTo(IDictionary<string, string> serializableInfo)
        {
            serializableInfo.Add("GoodsId", GoodsId.ToString());
            serializableInfo.Add("ReservationId", ReservationId.ToString());
        }
    }
}
