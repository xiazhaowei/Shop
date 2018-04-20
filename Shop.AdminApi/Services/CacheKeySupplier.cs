namespace Shop.AdminApi.Services
{
    /// <summary>
    /// 为缓存提供键key
    /// </summary>
    public class CacheKeySupplier
    {
        public static string UserModelCacheKey(string userId)
        {
            const string Key = "USERMODEL_";
            return Key + userId;
        }
        
    }
}