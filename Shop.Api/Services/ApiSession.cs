using Shop.QueryServices.Dtos;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Api.Services
{
    
    public class ApiSession
    {
        private volatile static ApiSession _instance = null;
        private static readonly object _lockHelper = new object();
        private ICacheHelper _cache;


        private ApiSession()
        {
            //缓存信息
            if (_cache == null)
            {
                _cache = new MemoryCacheHelper();
            }
        }

        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static ApiSession CreateInstance()
        {
            if (_instance == null)
            {
                lock (_lockHelper)
                {
                    if (_instance == null)
                        _instance = new ApiSession();
                }
            }
            return _instance;
        }

        #region 短信验证码
        /// <summary>
        /// 设置短信验证码到缓存
        /// </summary>
        /// <param name="token">key</param>
        /// <param name="code"></param>
        public void SetMsgCode(string token,string code)
        {
             _cache.SetCache(token, code);
        }
        /// <summary>
        /// 获取短信验证码缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetMsgCode(string token)
        {
            return _cache.GetCache<string>(token);
        }

        #endregion

        #region 用户信息

        #endregion

        #region 商品
        public void SetHomeNewGoodses(IEnumerable<GoodsAlias> goodses)
        {
            goodses.CheckNotNull(nameof(goodses));
            _cache.SetCache(CacheKeySupplier.HomeNewGoodsesCacheKey(), goodses);
        }
        public IEnumerable<GoodsAlias> GetHomeNewGoodses()
        {
            var goodses = _cache.GetCache<IEnumerable<GoodsAlias>>(CacheKeySupplier.HomeNewGoodsesCacheKey()) ;
            return goodses;
        }

        public void SetHomeRateGoodses(IEnumerable<GoodsAlias> goodses)
        {
            goodses.CheckNotNull(nameof(goodses));
            _cache.SetCache(CacheKeySupplier.HomeRateGoodsesCacheKey(), goodses);
        }
        public IEnumerable<GoodsAlias> GetHomeRateGoodses()
        {
            var goodses = _cache.GetCache<IEnumerable<GoodsAlias>>(CacheKeySupplier.HomeRateGoodsesCacheKey());
            return goodses;
        }

        public void SetHomeSelloutGoodses(IEnumerable<GoodsAlias> goodses)
        {
            goodses.CheckNotNull(nameof(goodses));
            _cache.SetCache(CacheKeySupplier.HomeSelloutGoodsesCacheKey(), goodses);
        }
        public IEnumerable<GoodsAlias> GetHomeSelloutGoodses()
        {
            var goodses = _cache.GetCache<IEnumerable<GoodsAlias>>(CacheKeySupplier.HomeSelloutGoodsesCacheKey());
            return goodses;
        }

        #endregion

        #region 善心指数
        public void SetBenevolenceIndex(string benevolenceIndex)
        {
            benevolenceIndex.CheckNotNull(nameof(benevolenceIndex));
            _cache.SetCache(CacheKeySupplier.BenevolenceIndexCacheKey(), benevolenceIndex);
        }
        public string GetBenevolenceIndex()
        {
            var benevolenceIndex = _cache.GetCache<string>(CacheKeySupplier.BenevolenceIndexCacheKey());
            return benevolenceIndex;
        }
        #endregion
        

    }
}