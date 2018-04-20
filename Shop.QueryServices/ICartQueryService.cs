using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface ICartQueryService
    {
        Cart Info(Guid id);

        IEnumerable<CartGoods> CartGoodses(Guid cartId);
    }
}