using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// 查询服务 实现
    /// </summary>
    [Component]
    public class GoodsBlockWarpQueryService : BaseQueryService,IGoodsBlockWarpQueryService
    {
        public GoodsBlockWarp Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                var goodsBlockWarp = connection.QueryList<GoodsBlockWarp>(new { Id = id }, ConfigSettings.GoodsBlockWarpTable).FirstOrDefault();
                if (goodsBlockWarp != null)
                {
                    goodsBlockWarp.GoodsBlocks = connection.QueryList<GoodsBlockWarpGoodBlock>(new { GoodsBlockWarpId = goodsBlockWarp.Id }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable).ToList();
                }
                return goodsBlockWarp;
            }
        }

        public IEnumerable<GoodsBlockWarp> All()
        {
            using (var connection = GetConnection())
            {
                var goodsBlockWarps= connection.QueryList<GoodsBlockWarp>(null, ConfigSettings.GoodsBlockWarpTable);
                foreach(var goodsBlockWarp in goodsBlockWarps)
                {
                    goodsBlockWarp.GoodsBlocks= connection.QueryList<GoodsBlockWarpGoodBlock>(new { GoodsBlockWarpId = goodsBlockWarp.Id }, ConfigSettings.GoodsBlockWarpGoodsBlocksTable).ToList();
                }
                return goodsBlockWarps;
            }
        }

        public IEnumerable<GoodsBlockWarpAlis> AllAlis()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<GoodsBlockWarpAlis>(null, ConfigSettings.GoodsBlockWarpTable);
            }
        }
    }
}