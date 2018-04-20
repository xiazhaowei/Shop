using Dapper;
using ECommon.Components;
using ECommon.Dapper;
using Shop.Common;
using Shop.Common.Enums;
using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.QueryServices.Dapper
{
    /// <summary>
    /// 产品查询服务 实现
    /// </summary>
    [Component]
    public class GoodsQueryService : BaseQueryService,IGoodsQueryService
    {
        public GoodsDetails GetGoodsDetails(Guid goodsId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<GoodsDetails>(new { Id = goodsId }, ConfigSettings.GoodsTable).SingleOrDefault();
            }
        }
        public GoodsAlias GetGoodsAlias(Guid goodsId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<GoodsAlias>(new { Id = goodsId }, ConfigSettings.GoodsTable).SingleOrDefault();
            }
        }

        public IEnumerable<GoodsAlias> GetPublishedGoodses()
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<GoodsAlias>(new { IsPublished = 1,Status=GoodsStatus.Verifyed }, ConfigSettings.GoodsTable);
            }
        }

        /// <summary>
        /// 热卖产品
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<GoodsAlias> GoodSellGoodses(int count)
        {
            var sql = string.Format(@"select Id,Pics,Name,Price,OriginalPrice,Benevolence,SellOut,Rate,CreatedOn from {0} 
                where  IsPublished=1  and Status=1
                order by SellOut desc", ConfigSettings.GoodsTable);
            using (var connection = GetConnection())
            {
                return connection.Query<GoodsAlias>(sql).Take(count);
            }
        }

        
        /// <summary>
        /// 新品
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<GoodsAlias> NewGoodses(int count)
        {
            var sql = string.Format(@"select Id,Pics,Name,Price,OriginalPrice,Benevolence,SellOut,Rate,CreatedOn from {0} 
                where  IsPublished=1 and Status=1
                order by CreatedOn desc", ConfigSettings.GoodsTable);
            using (var connection = GetConnection())
            {
                return connection.Query<GoodsAlias>(sql).Take(count);
            }
        }

        /// <summary>
        /// 好评产品
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<GoodsAlias> GoodRateGoodses(int count)
        {
            var sql = string.Format(@"select Id,Pics,Name,Price,OriginalPrice,Benevolence,SellOut,Rate,CreatedOn from {0} 
                where  IsPublished=1 and Status=1
                order by Rate desc", ConfigSettings.GoodsTable);
            using (var connection = GetConnection())
            {
                return connection.Query<GoodsAlias>(sql).Take(count);
            }
        }
        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<GoodsAlias> Search(string search)
        {
            var sql = string.Format(@"select Id,Pics,Name,Price,OriginalPrice,Benevolence,SellOut,Rate,CreatedOn from {0} 
                where Name like '%{1}%' and IsPublished=1 and Status=1", ConfigSettings.GoodsTable,search);
            using (var connection = GetConnection())
            {
                return connection.Query<GoodsAlias>(sql);
            }
        }

        /// <summary>
        /// 获取分类下 的商品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<GoodsAlias> CategoryGoodses(Guid categoryId)
        {
            var sql = string.Format(@"select a.Id,a.Pics,a.Name,a.Price,a.OriginalPrice,a.Benevolence,a.SellOut,a.Rate,a.CreatedOn  
            from {0} as a inner join {1} as b on a.Id=b.GoodsId
                where b.CategoryId='{2}' and a.IsPublished=1 and a.Status=1", ConfigSettings.GoodsTable, ConfigSettings.GoodsPubCategorysTable, categoryId);
            using (var connection = GetConnection())
            {
                return connection.Query<GoodsAlias>(sql);
            }
        }

        /// <summary>
        /// 产品最近的几个评论
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetComments(Guid goodsId,int count)
        {
            var sql = string.Format(@"select top({2}) a.Id
            ,a.UserId
            ,a.[Body]
            ,a.[CreatedOn]
            ,a.Rate
            ,a.Thumbs
            ,b.NickName as NickName 
            from {0} a inner join {1} b on a.UserId=b.Id 
            where a.GoodsId='{3}' 
            order by a.CreatedOn desc", ConfigSettings.GoodsCommentsTable, ConfigSettings.UserTable,count,goodsId);

            using (var connection = GetConnection())
            {
                return connection.Query<Comment>(sql);
            }
        }

        //获取默认规格
        public Specification GetGoodsDefaultSpecification(Guid goodsId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Specification>(new {GoodsId = goodsId,Name="默认规格",Value= "默认规格" }, ConfigSettings.SpecificationTable).SingleOrDefault();
            }
        }

        public IEnumerable<GoodsDetails> GetStoreGoodses(Guid storeId)
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format("select a.*,b.Name as StoreName " +
                    "from {0} as a inner join {1} as b on a.StoreId=b.Id " +
                    "where a.StoreId='{2}'", ConfigSettings.GoodsTable, ConfigSettings.StoreTable,storeId);

                return connection.QueryList<GoodsDetails>(new { StoreId = storeId }, ConfigSettings.GoodsTable);
            }
        }

        /// <summary>
        /// 所有商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoodsDetails> Goodses()
        {
            using (var connection = GetConnection())
            {
                var sql = string.Format("select a.*,b.Name as StoreName from {0} as a inner join {1} as b on a.StoreId=b.Id",ConfigSettings.GoodsTable,ConfigSettings.StoreTable);
                return connection.Query<GoodsDetails>(sql);
            }
        }

        public IEnumerable<GoodsParam> GetGoodsParams(Guid goodsId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<GoodsParam>(new { GoodsId = goodsId }, ConfigSettings.GoodsParamTable);
            }
        }

        public IEnumerable<Specification> GetPublishedSpecifications(Guid goodsId)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryList<Specification>(new { GoodsId = goodsId }, ConfigSettings.SpecificationTable);
            }
        }
        public IEnumerable<SpecificationName> GetSpecificationNames(IEnumerable<Guid> specifications)
        {
            var distinctIds = specifications.Distinct().ToArray();
            if (distinctIds.Length == 0)
            {
                return new List<SpecificationName>();
            }

            using (var connection = GetConnection())
            {
                var result = new List<SpecificationName>();
                foreach (var specificationId in distinctIds)
                {
                    var specification = connection.QueryList<Specification>(new { Id = specificationId }, ConfigSettings.SpecificationTable).SingleOrDefault();
                    if (specification != null)
                    {
                        result.Add(new SpecificationName { Id = specification.Id, Name = specification.Name });
                    }
                }
                return result;
            }
        }

       

    }
}