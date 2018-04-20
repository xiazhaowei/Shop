namespace Shop.AdminApi.Services
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
        
    }
}