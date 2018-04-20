using Shop.QueryServices.Dtos;
using System;
using System.Collections.Generic;

namespace Shop.QueryServices
{
    /// <summary>
    /// 产品查询服务接口
    /// </summary>
    public interface IGoodsQueryService
    {
        GoodsDetails GetGoodsDetails(Guid goodsId);
        GoodsAlias GetGoodsAlias(Guid goodsId);

        IEnumerable<GoodsDetails> GetStoreGoodses(Guid storeId);

        IEnumerable<GoodsAlias> GetPublishedGoodses();

        IEnumerable<GoodsAlias> Search(string search);
        IEnumerable<GoodsAlias> CategoryGoodses(Guid categoryId);

        IEnumerable<GoodsAlias> GoodSellGoodses(int count);
        IEnumerable<GoodsAlias> GoodRateGoodses(int count);
        IEnumerable<GoodsAlias> NewGoodses(int count);

        IEnumerable<GoodsDetails> Goodses();

        IEnumerable<Comment> GetComments(Guid goodsId,int count);

        IEnumerable<GoodsParam> GetGoodsParams(Guid goodsId);

        IEnumerable<Specification> GetPublishedSpecifications(Guid goodsId);
        IEnumerable<SpecificationName> GetSpecificationNames(IEnumerable<Guid> specifications);

        Specification GetGoodsDefaultSpecification(Guid goodsId);
    }
}