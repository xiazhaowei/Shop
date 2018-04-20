using Shop.QueryServices.Dtos;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    public interface IOrderGoodsQueryService
    {
        IEnumerable<OrderGoodsAlis> ExpiredNormalGoodses();
    }
}
