using Dapper;
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
    public class GoodsBlockQueryService : BaseQueryService,IGoodsBlockQueryService
    {
        public GoodsBlock Find(Guid id)
        {
            using (var connection = GetConnection())
            {
                var goodsBlock= connection.QueryList<GoodsBlock>(new { Id = id }, ConfigSettings.GoodsBlockTable).FirstOrDefault();
                if (goodsBlock != null)
                {
                    var sql = string.Format(@"select b.Id,
                        b.Name,
                        b.Pics,
                        b.Price,
                        b.OriginalPrice,
                        b.Rate,
                        b.SellOut,
                        b.CreatedOn,
                        b.Benevolence 
                        from {0} as a inner join {1} as b on a.GoodsId=b.Id 
                        where a.GoodsBlockId='{2}' and b.IsPublished=1 and b.Status=1", ConfigSettings.GoodsBlockGoodsesTable,ConfigSettings.GoodsTable, goodsBlock.Id);
                    goodsBlock.Goodses = connection.Query<GoodsAlias>(sql).ToList();
                }
                return goodsBlock;
            }
            
        }

        public IEnumerable<GoodsBlock> All()
        {
            using (var connection = GetConnection())
            {
                var goodsBlocks = connection.QueryList<GoodsBlock>(null, ConfigSettings.GoodsBlockTable);
                var sql = "";
                foreach (var goodsBlock in goodsBlocks)
                {
                    sql = string.Format(@"select b.Id,
                        b.Name,
                        b.Pics,
                        b.Price,
                        b.OriginalPrice,
                        b.Rate,
                        b.SellOut,
                        b.CreatedOn,
                        b.Benevolence 
                        from {0} as a inner join {1} as b on a.GoodsId=b.Id 
                        where a.GoodsBlockId='{2}' and b.Status=1 and b.IsPublished=1", ConfigSettings.GoodsBlockGoodsesTable, ConfigSettings.GoodsTable, goodsBlock.Id);
                    goodsBlock.Goodses = connection.Query<GoodsAlias>(sql).ToList();
                }
                return goodsBlocks;
            }
        }

        public IEnumerable<GoodsBlock> GetList(Guid goodsBlockWarpId)
        {
            var sql = string.Format(@"select b.* 
                from {0} as a left join {1} as b on a.GoodsBlockId=b.Id 
                where a.GoodsBlockWarpId='{2}'", ConfigSettings.GoodsBlockWarpGoodsBlocksTable, ConfigSettings.GoodsBlockTable, goodsBlockWarpId);

            using (var connection = GetConnection())
            {
                var goodsBlocks= connection.Query<GoodsBlock>(sql);
                string sql2 = "";
                foreach (var goodsBlock in goodsBlocks)
                {
                        sql2 = string.Format(@"select 
                        b.Id,
                        b.Name,
                        b.Pics,
                        b.Price,
                        b.OriginalPrice,
                        b.Benevolence,
                        b.SellOut,
                        b.Rate,
                        b.CreatedOn 
                        from {0} as a inner join {1} as b on a.GoodsId=b.Id 
                        where a.GoodsBlockId='{2}'  and b.Status=1 and b.IsPublished=1", ConfigSettings.GoodsBlockGoodsesTable, ConfigSettings.GoodsTable, goodsBlock.Id);

                    goodsBlock.Goodses = connection.Query<GoodsAlias>(sql2).ToList();
                }
                return goodsBlocks;
            }
        }
    }
}