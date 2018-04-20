using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// 查询服务接口
    /// </summary>
    public interface IGoodsBlockWarpQueryService
    {
        GoodsBlockWarp Find(Guid id);
        IEnumerable<GoodsBlockWarp> All();
        IEnumerable<GoodsBlockWarpAlis> AllAlis();
    }
}