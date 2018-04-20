namespace Shop.Api.Services
{
    /// <summary>
    /// 为缓存提供键key
    /// </summary>
    public class CacheKeySupplier
    {
        /// <summary>
        /// 店铺模型缓存key
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public static string StoreModelCacheKey(string storeId)
        {
            const string Key = "STOREMODEL_";
            return Key + storeId;
        }

        /// <summary>
        /// 钱包模型缓存key
        /// </summary>
        /// <param name="walletId"></param>
        /// <returns></returns>
        public static string WalletModelCacheKey(string walletId)
        {
            const string Key = "WALLETMODEL_";
            return Key + walletId;
        }

        public static string CartModelCacheKey(string cartId)
        {
            const string Key = "CARTMODEL_";
            return Key + cartId;
        }

        /// <summary>
        /// 用户模型缓存key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string UserModelCacheKey(string userId)
        {
            const string Key = "USERMODEL_";
            return Key + userId;
        }

        public static string HomeNewGoodsesCacheKey()
        {
            return "HOMENEWGOOSES";
        }
        public static string HomeRateGoodsesCacheKey()
        {
            return "HOMERATEGOOSES";
        }
        public static string HomeSelloutGoodsesCacheKey()
        {
            return "HOMESELLOUTGOOSES";
        }

        public static string BenevolenceIndexCacheKey()
        {
            return "BENEVOLENCEINDEX";
        }
    }
}