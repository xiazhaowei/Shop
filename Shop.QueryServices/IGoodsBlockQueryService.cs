using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// ��ѯ����ӿ�
    /// </summary>
    public interface IGoodsBlockQueryService
    {
        GoodsBlock Find(Guid id);
        IEnumerable<GoodsBlock> All();
        IEnumerable<GoodsBlock> GetList(Guid goodsBlockWarpId);
    }
}