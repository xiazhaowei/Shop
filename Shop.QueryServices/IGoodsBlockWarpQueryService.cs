using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface IGoodsBlockWarpQueryService
    {
        GoodsBlockWarp Find(Guid id);
        IEnumerable<GoodsBlockWarp> All();
        IEnumerable<GoodsBlockWarpAlis> AllAlis();
    }
}